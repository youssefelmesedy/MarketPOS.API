using MarketPOS.Application.InterfaceCacheing;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Concurrent;

namespace MarketPOS.Infrastructure.ImplmentationCacheing;

public class MemoryCacheService : IGenericCache
{
    private readonly IMemoryCache _memoryCache;
    private readonly ILogger<MemoryCacheService> _logger;
    private readonly IStringLocalizer<MemoryCacheService> _localizer;

    // هنا هنسجل الكيز المرتبطة بكل Prefix
    private readonly ConcurrentDictionary<string, HashSet<string>> _prefixKeys = new();

    public MemoryCacheService(
        IMemoryCache memoryCache,
        ILogger<MemoryCacheService> logger,
        IStringLocalizer<MemoryCacheService> localizer)
    {
        _memoryCache = memoryCache;
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
                    return exp.ToString(); // ثابت ومش بيتغير بين Calls
                case Delegate del:
                    return del.Method.ToString(); // اسم الميثود
                default:
                    return p.ToString() ?? "null";
            }
        }));
    }

    public Task<T?> GetAsync<T>(string key)
    {
        try
        {
            if (_memoryCache.TryGetValue(key, out T? value))
            {
                _logger.LogInformation(_localizer["Success"] + $" (Cache Hit): Key = {key}");
                return Task.FromResult<T?>(value);
            }

            _logger.LogWarning(_localizer["NotFound"] + $" (Cache Miss) Key={key}");
            return Task.FromResult<T?>(default);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, _localizer["GetByIdFailed"] + $" (Key={key})");
            return Task.FromResult<T?>(default);
        }
    }

    public Task SetAsync<T>(string key, T value, TimeSpan? absoluteExpiration = null)
    {
        try
        {
            var options = new MemoryCacheEntryOptions();
            if (absoluteExpiration.HasValue)
                options.AbsoluteExpirationRelativeToNow = absoluteExpiration;

            _memoryCache.Set(key, value, options);

            // سجل المفتاح ضمن الـ prefix
            var prefix = key.Split('_').First();
            _prefixKeys.AddOrUpdate(prefix,
                _ => new HashSet<string> { key },
                (_, set) => { set.Add(key); return set; });

            _logger.LogInformation(_localizer["Created"] + $" (Cache SetAsync) Key = {key}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, _localizer["CreateFailed"] + $" (Key = {key})");
        }

        return Task.CompletedTask;
    }

    public Task RemoveAsync(string key)
    {
        try
        {
            _memoryCache.Remove(key);

            var prefix = key.Split('_').First();
            if (_prefixKeys.TryGetValue(prefix, out var set))
                set.Remove(key);

            _logger.LogInformation(_localizer["Deleted"] + $" (Cache Remove) Key = {key}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, _localizer["DeleteFailed"] + $" (Key = {key})");
        }

        return Task.CompletedTask;
    }

    public Task RemoveByPrefixAsync(string prefix)
    {
        try
        {
            if (_prefixKeys.TryRemove(prefix, out var keys))
            {
                foreach (var key in keys)
                    _memoryCache.Remove(key);

                _logger.LogInformation(_localizer["Deleted"] + $" (Cache RemoveByPrefix) Prefix = {prefix}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, _localizer["DeleteFailed"] + $" (Prefix = {prefix})");
        }

        return Task.CompletedTask;
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
