namespace MarketPOS.Application.InterfaceCacheing;
public interface IGenericCache
{
    string BuildCacheKey(params object?[] parts);
    Task<T?> GetAsync<T>(string key);
    Task SetAsync<T>(string key, T value, TimeSpan? absoluteExpiration = null);
    Task RemoveAsync(string key);

    Task<T> GetOrAddAsync<T>(string key, Func<Task<T>> factory, TimeSpan? absoluteExpiration = null);
}
