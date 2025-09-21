using Microsoft.AspNetCore.Http;

namespace MarketPOS.Infrastructure.TrackingServicesMiddleware;

public class ServiceTrackingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ServiceTrackingMiddleware> _logger;

    public ServiceTrackingMiddleware(RequestDelegate next, ILogger<ServiceTrackingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        finally
        {
            var resolved = ServiceTracker.GetResolvedServices();
            if (resolved.Any())
            {
                _logger.LogInformation("\n\n\t\t✅ Services resolved this request: {Count}", resolved.Count);
                foreach (var kvp in resolved)
                {
                    _logger.LogInformation("\t\t - {ServiceName}: {Count} times\n\n", kvp.Key, kvp.Value);
                }
            }
            else
            {
                _logger.LogInformation("\t\t⚠️ No services were resolved in this request.\n\n");
            }

            // نفضي الـ Cache بعد ما الـ Request يخلص
            ServiceTracker.Clear();
        }
    }
}


