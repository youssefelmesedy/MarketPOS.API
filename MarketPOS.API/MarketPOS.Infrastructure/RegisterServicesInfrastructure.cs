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
