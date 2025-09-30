using MarketPOS.Application.InterfaceCacheing;
using MarketPOS.Infrastructure.ImplmentationCacheing;

namespace MarketPOS.API.Bootstrapper.Extensions.ExtensionCacheing;
public static class CachingExtensions
{
    public static IServiceCollection AddCustomCaching(this IServiceCollection services, IConfiguration configuration)
    {
        var cacheProvider = configuration["CacheSettings:Provider"];

        if (cacheProvider?.Equals("Redis", StringComparison.OrdinalIgnoreCase) == true)
        {
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration["CacheSettings:RedisConnection"];
                options.InstanceName = "MarketPOS_";
            });

            services.AddScoped<IGenericCache, RedisCacheService>();
        }
        else
        {
            services.AddMemoryCache();
            services.AddScoped<IGenericCache, MemoryCacheService>();
        }

        return services;
    }
}
