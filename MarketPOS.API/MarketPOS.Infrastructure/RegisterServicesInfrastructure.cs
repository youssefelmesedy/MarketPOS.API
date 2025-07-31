using MarketPOS.Application.RepositoryInterfaces;
using MarketPOS.Application.RepositoryInterfaces.ProductRepositorys;
using MarketPOS.Application.RepositoryInterfaces.RepositoryCategory;
using MarketPOS.Application.RepositoryInterfaces.RepositorySupplier;
using MarketPOS.Application.Services.InterfacesServices.EntityIntrerfaceService;
using MarketPOS.Application.Specifications;

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
        services.AddScoped(typeof(ISpecificationEvaluator<>), typeof(SpecificationEvaluator<>));
        services.AddScoped(typeof(IFullRepository<>), typeof(GenericeRepository<>));
        services.AddScoped(typeof(IReadOnlyRepository<>), typeof(GenericeRepository<>));
        services.AddScoped(typeof(IFullService<>), typeof(GenericService<>));
        services.AddScoped(typeof(IReadOnlyService<>), typeof(GenericService<>));

        services.AddScoped<IProductRepo, ProductRepository>();
        services.AddScoped<IProductPriceRepo, ProductPriceRepository>();
        services.AddScoped<IProductUnitProfileRepo, ProductUnitProfileRepository>();
        services.AddScoped<ICategoryRepo, CategoryRepository>();
        services.AddScoped<ISupplierRepo, SupplierRepo>();

        //✅ تسجيل Services
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IProductPriceService, ProductPriceService>(); 
        services.AddScoped<IProductUnitProfileService, ProductUnitProfileService>(); 
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<ISupplierService, SupplierService>(); 

        return services;
    }
}
