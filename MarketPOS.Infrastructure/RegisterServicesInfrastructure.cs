namespace MarketPOS.Infrastructure;

public static class RegisterServicesInfrastructure
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        // ✅ تسجيل DbContext    
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")).EnableSensitiveDataLogging());

        // ✅ تسجيل UnitOfWork
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // ✅ تسجيل الريبو الجينيريك
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<IProductRepo, ProductRepository>();
        services.AddScoped<ICategoryRepo, CategoryRepository>();
        services.AddScoped<ISupplierRepo, SupplierRepo>();

        //✅ تسجيل Services
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<ISupplierService, SupplierService>(); 

        return services;
    }
}
