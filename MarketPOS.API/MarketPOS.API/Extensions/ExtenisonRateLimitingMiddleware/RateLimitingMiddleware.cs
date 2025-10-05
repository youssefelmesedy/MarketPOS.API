using MarketPOS.API.ExtensionsFiltreingAndMiddlewares.ExtensionAttribute;
using MarketPOS.Application.InterfaceCacheing;
using MarketPOS.Shared.RateLimitedSettings;
using Microsoft.Extensions.Options;
using System.Security.Claims;

public partial class RateLimitingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RateLimitingMiddleware> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly RateLimitingOptions _options;

    public RateLimitingMiddleware(
        RequestDelegate next,
        ILogger<RateLimitingMiddleware> logger,
        IServiceProvider serviceProvider,
        IOptions<RateLimitingOptions> options)
    {
        _next = next;
        _logger = logger;
        _serviceProvider = serviceProvider;
        _options = options.Value;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var endpoint = context.GetEndpoint();
        var rateLimitAttr = endpoint?.Metadata.GetMetadata<RateLimitAttribute>();

        // 🔹 إنشاء scope جديد لاستخدام خدمات Scoped مثل الكاش
        using var scope = _serviceProvider.CreateScope();
        var _cache = scope.ServiceProvider.GetRequiredService<IGenericCache>();

        var userRole = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value ?? "Guest";
        var userId = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value
                     ?? context.Connection.RemoteIpAddress?.ToString()
                     ?? "Anonymous";

        _logger.LogInformation(
            $"\n\n===================== 🧩 RATE LIMIT CHECK START =====================\n" +
            $"👤 UserId: {userId}\n" +
            $"🎭 Role: {userRole}\n" +
            $"📍 Endpoint: {endpoint?.DisplayName}\n" +
            $"=====================================================================\n\n"
        );

        // 1️⃣ لو فيه RateLimitAttribute نستخدمه
        if (rateLimitAttr != null && rateLimitAttr.RoleLimits.TryGetValue(userRole, out var rule))
        {
            _logger.LogInformation(
                $"\n\n📌 Using RateLimitAttribute configuration → " +
                $"Limit={rule.Limit}, Period={rule.Seconds}s\n\n"
            );

            await ApplyRateLimitingAsync(context, endpoint, userRole, userId, rule, _cache);
            return;
        }

        // 2️⃣ لو مفيش، استخدم الإعدادات من AppSettings
        var appRule = GetRuleFromSettings(userRole, userId);

        _logger.LogInformation(
            $"\n\n📜 Using appsettings.ratelimit.json configuration → " +
            $"Limit={appRule.Limit}, Period={appRule.Seconds}s\n\n"
        );

        await ApplyRateLimitingAsync(context, endpoint, userRole, userId, appRule, _cache);
    }

    private (int Limit, int Seconds) GetRuleFromSettings(string role, string userId)
    {
        if (_options.Admins.TryGetValue(userId, out var adminRule))
            return (adminRule.MaxRequests, adminRule.TimeWindowSeconds);

        if (_options.Users.TryGetValue(userId, out var userRule))
            return (userRule.MaxRequests, userRule.TimeWindowSeconds);

        if (_options.Guests.TryGetValue(userId, out var guestRule))
            return (guestRule.MaxRequests, guestRule.TimeWindowSeconds);

        if (_options.SpecialRoles.TryGetValue(role, out var specialRule))
            return (specialRule.MaxRequests, specialRule.TimeWindowSeconds);

        return (_options.Default.MaxRequests, _options.Default.TimeWindowSeconds);
    }

    private async Task ApplyRateLimitingAsync(
        HttpContext context,
        Endpoint? endpoint,
        string userRole,
        string userId,
        (int Limit, int Seconds) rule,
        IGenericCache _cache)
    {
        var (limit, seconds) = rule;
        var cacheKey = _cache.BuildCacheKey("RateLimit", endpoint?.DisplayName, userRole, userId);
        var period = TimeSpan.FromSeconds(seconds);
        var now = DateTime.UtcNow;

        var entry = await _cache.GetAsync<RateLimitEntry>(cacheKey);

        if (entry == null)
        {
            entry = new RateLimitEntry { Count = 1, WindowStartUtc = now };
            await _cache.SetAsync(cacheKey, entry, period);

            _logger.LogInformation(
                $"\n\n✅ First request detected.\n" +
                $"🔑 CacheKey: {cacheKey}\n" +
                $"📊 Count: 1/{limit}\n" +
                $"⏱️ Period: {seconds}s\n" +
                $"🕒 WindowStart: {entry.WindowStartUtc}\n\n"
            );

            await _next(context);
            return;
        }

        if (now - entry.WindowStartUtc >= period)
        {
            entry.Count = 1;
            entry.WindowStartUtc = now;
            await _cache.SetAsync(cacheKey, entry, period);

            _logger.LogInformation(
                $"\n\n🔄 Rate limit window reset.\n" +
                $"📊 Count reset to: 1/{limit}\n" +
                $"🕒 New window start: {entry.WindowStartUtc}\n" +
                $"⏱️ Period: {seconds}s\n\n"
            );

            await _next(context);
            return;
        }

        if (entry.Count >= limit)
        {
            var resetAt = entry.WindowStartUtc.Add(period);
            var retryAfter = (int)Math.Ceiling((resetAt - now).TotalSeconds);

            _logger.LogWarning(
                $"\n\n🚫 RATE LIMIT EXCEEDED!\n" +
                $"📊 Count: {entry.Count}/{limit}\n" +
                $"🕒 Reset At (UTC): {resetAt}\n" +
                $"⏳ Retry After: {retryAfter}s\n" +
                $"🔑 CacheKey: {cacheKey}\n\n"
            );

            context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
            context.Response.Headers["Retry-After"] = retryAfter.ToString();
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsJsonAsync(new
            {
                title = "Too Many Requests",
                status = 429,
                detail = $"Rate limit exceeded for {userRole} ({userId}). Retry after {retryAfter}s",
                instance = context.Request.Path
            });

            return;
        }

        entry.Count++;
        var remaining = entry.WindowStartUtc.Add(period) - now;
        await _cache.SetAsync(cacheKey, entry, remaining);

        _logger.LogInformation(
            $"\n\n✅ Request allowed.\n" +
            $"📊 Count: {entry.Count}/{limit}\n" +
            $"⏱️ Period: {seconds}s\n" +
            $"🕒 Remaining: {remaining.TotalSeconds:F1}s\n" +
            $"🔑 CacheKey: {cacheKey}\n\n"
        );

        await _next(context);
    }
}
