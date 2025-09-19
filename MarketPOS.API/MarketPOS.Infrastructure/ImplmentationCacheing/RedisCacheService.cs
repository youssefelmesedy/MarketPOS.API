using MarketPOS.Application.InterfaceCacheing;
using Microsoft.Extensions.Caching.Distributed;

namespace MarketPOS.Infrastructure.ImplmentationCacheing;


public class RedisCacheService : IGenericCache
{
    private readonly IDistributedCache _cache;
    private readonly ILogger<RedisCacheService> _logger;
    private readonly IStringLocalizer<RedisCacheService> _localizer;

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

            // لو object عبارة عن expression أو delegate
            var type = p.GetType();
            if (type.IsSubclassOf(typeof(Delegate)) || type.Name.Contains("Expression"))
            {
                return p.GetHashCode().ToString(); // بدل ToString
            }

            return p.ToString();
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
            _logger.LogInformation(_localizer["Deleted"] + $" (Cache Remove) Key={key}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, _localizer["DeleteFailed"] + $" (Key={key})");
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

