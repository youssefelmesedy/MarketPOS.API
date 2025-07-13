namespace MarketPOS.Infrastructure;

public static class RegisterServicesInfrastructure
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        // ✅ تسجيل DbContext    
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), sqlOptions =>
            {
                sqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
            });

            #if DEBUG
            options.EnableSensitiveDataLogging(); // ✅ تشغيل في بيئة التطوير فقط
            #endif 
        });



        // ✅ تسجيل UnitOfWork
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // ✅ تسجيل الريبو الجينيريك
        services.AddScoped(typeof(IQueryableRepository<>), typeof(GenericeRepository<>));
        services.AddScoped(typeof(IProjectableRepository<>), typeof(GenericeRepository<>));
        services.AddScoped(typeof(IWritableRepository<>), typeof(GenericeRepository<>));
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
