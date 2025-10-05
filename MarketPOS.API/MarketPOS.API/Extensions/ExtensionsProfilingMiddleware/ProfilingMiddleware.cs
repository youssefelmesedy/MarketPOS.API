using MarketPOS.Application.InterfaceCacheing;
using System.Diagnostics;

namespace MarketPOS.API.Extensions.ExtensionsProfilingMiddleware;

public class ProfilingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ProfilingMiddleware> _logger;
    private readonly string _cacheKeyPrefix;

    public ProfilingMiddleware(RequestDelegate next, ILogger<ProfilingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
        _cacheKeyPrefix = nameof(ProfilingMiddleware);
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var cache = context.RequestServices.GetRequiredService<IGenericCache>();
        var endpoint = context.GetEndpoint();

        var endpointName = endpoint?.DisplayName ?? context.Request.Path.ToString();
        var cacheKey = cache.BuildCacheKey(_cacheKeyPrefix, endpointName);

        // 🟢 جلب عدد الطلبات الحالي من الكاش
        var requestCount = await cache.GetAsync<int>(cacheKey);
        requestCount++; // زيادة العدد

        // 🟢 تحديث الكاش
        await cache.SetAsync(cacheKey, requestCount, TimeSpan.FromHours(1));

        // 🟢 بدء العد بالساعة
        var stopwatch = Stopwatch.StartNew();

        // تنفيذ بقية الـ pipeline
        await _next(context);

        stopwatch.Stop();
        var elapsedMs = stopwatch.ElapsedMilliseconds;

        // 🟢 تسجيل الـ logs
        _logger.LogInformation(
            "\n==============================\n" +
            $"⏱️ Endpoint: {endpointName}\n" +
            $"🔢 Request Count: {requestCount}\n" +
            $"⏳ Execution Time: {elapsedMs} ms\n" +
            "=============================="
        );
    }
}

