using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Collections.Concurrent;

namespace MarketPOS.Infrastructure;

public static class RegisterServicesInfrastructure
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), sqlOptions =>
            {
                sqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);

                sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,                       
                    maxRetryDelay: TimeSpan.FromSeconds(10),
                    errorNumbersToAdd: null                 
                );
            });
        });

        // Debandancey Injection for UnitOfWork and Repositories
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Debandancey Injection for Generic Repositories and Decorate Caching
        services.AddScoped(typeof(IFullRepository<>), typeof(GenericeRepository<>));
        services.AddScoped(typeof(IFullService<>), typeof(GenericService<>));
        services.AddScoped(typeof(IFullService<>), typeof(GenericServiceCacheing<>));

        services.AddScoped<IProductRepo, ProductRepository>();
        services.AddScoped<IProductPriceRepo, ProductPriceRepository>();
        services.AddScoped<IProductUnitProfileRepo, ProductUnitProfileRepository>();
        services.AddScoped<ICategoryRepo, CategoryRepository>();
        services.AddScoped<IActivelngredinentsRepo, ActiveingredinentRepository>();
        services.AddScoped<IWareHouseRepo, WareHouseRepository>();
        services.AddScoped<ISupplierRepo, SupplierRepo>();

        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IProductPriceService, ProductPriceService>();
        services.AddScoped<IProductUnitProfileService, ProductUnitProfileService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IActiveingredinentService, ActiveingredinentService>();
        services.AddScoped<IWareHouseService, WareHouseService>();
        services.AddScoped<ISupplierService, SupplierService>();
        services.AddScoped<IAggregateService, AggregateService>();

        return services;
    }
}
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
                _logger.LogInformation("✅ Services resolved this request: {Count}", resolved.Count);
                foreach (var kvp in resolved)
                {
                    _logger.LogInformation(" - {ServiceName}: {Count} times", kvp.Key, kvp.Value);
                }
            }
            else
            {
                _logger.LogInformation("⚠️ No services were resolved in this request.");
            }

            // نفضي الـ Cache بعد ما الـ Request يخلص
            ServiceTracker.Clear();
        }
    }
}

public static class ServiceTrackingMiddlewareExtensions
{
    public static IApplicationBuilder UseServiceTracking(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ServiceTrackingMiddleware>();
    }
}
public static class ServiceTracker
{
    private static readonly ConcurrentDictionary<string, int> _resolvedServices = new();

    public static void Add(string serviceName)
    {
        _resolvedServices.AddOrUpdate(serviceName, 1, (_, count) => count + 1);
    }

    public static IReadOnlyDictionary<string, int> GetResolvedServices()
    {
        return _resolvedServices;
    }

    public static void Clear()
    {
        _resolvedServices.Clear();
    }
}


