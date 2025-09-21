using MarketPOS.Application.InterfaceCacheing;
using Microsoft.Extensions.Caching.Distributed;
using System.Collections.Concurrent;
using System.Text.Json;

namespace MarketPOS.Infrastructure.ImplmentationCacheing;

public class RedisCacheService : IGenericCache
{
    private readonly IDistributedCache _cache;
    private readonly ILogger<RedisCacheService> _logger;
    private readonly IStringLocalizer<RedisCacheService> _localizer;

    // لتتبع الكيز المرتبطة بكل Prefix
    private static readonly ConcurrentDictionary<string, HashSet<string>> _prefixKeys = new();

    public RedisCacheService(
        IDistributedCache cache,
        ILogger<RedisCacheService> logger,
        IStringLocalizer<RedisCacheService> localizer)
    {
        _cache = cache;
        _logger = logger;
        _localizer = localizer;
    }

    public string BuildCacheKey(params object?[] parts)
    {
        return string.Join("_", parts.Select(p =>
        {
            if (p == null) return "null";

            switch (p)
            {
                case Expression exp:
                    return exp.ToString(); // ثابت
                case Delegate del:
                    return del.Method.ToString(); // اسم الميثود
                default:
                    return p.ToString() ?? "null";
            }
        }));
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        try
        {
            var data = await _cache.GetStringAsync(key);

            if (data is null)
            {
                _logger.LogWarning(_localizer["NotFound"] + $" (Cache Miss) Key={key}");
                return default;
            }

            var value = JsonSerializer.Deserialize<T>(data);
            _logger.LogInformation(_localizer["Success"] + $" (Cache Hit) Key={key}");
            return value;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, _localizer["GetByIdFailed"] + $" (Key={key})");
            return default;
        }
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? absoluteExpiration = null)
    {
        try
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = absoluteExpiration
            };

            var data = JsonSerializer.Serialize(value);
            await _cache.SetStringAsync(key, data, options);

            // سجل المفتاح ضمن الـ prefix
            var prefix = key.Split('_').First();
            _prefixKeys.AddOrUpdate(prefix,
                _ => new HashSet<string> { key },
                (_, set) => { set.Add(key); return set; });

            _logger.LogInformation(_localizer["Created"] + $" (Cache SetAsync) Key={key}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, _localizer["CreateFailed"] + $" (Key={key})");
        }
    }

    public async Task RemoveAsync(string key)
    {
        try
        {
            await _cache.RemoveAsync(key);

            var prefix = key.Split('_').First();
            if (_prefixKeys.TryGetValue(prefix, out var set))
                set.Remove(key);

            _logger.LogInformation(_localizer["Deleted"] + $" (Cache Remove) Key={key}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, _localizer["DeleteFailed"] + $" (Key={key})");
        }
    }

    public async Task RemoveByPrefixAsync(string prefix)
    {
        try
        {
            if (_prefixKeys.TryRemove(prefix, out var keys))
            {
                foreach (var key in keys)
                    await _cache.RemoveAsync(key);

                _logger.LogInformation(_localizer["Deleted"] + $" (Cache RemoveByPrefix) Prefix={prefix}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, _localizer["DeleteFailed"] + $" (Prefix={prefix})");
        }
    }

    public async Task<T> GetOrAddAsync<T>(string key, Func<Task<T>> factory, TimeSpan? absoluteExpiration = null)
    {
        var existing = await GetAsync<T>(key);
        if (existing != null)
            return existing;

        var value = await factory();
        if (value != null)
            await SetAsync(key, value, absoluteExpiration);

        return value!;
    }
}
