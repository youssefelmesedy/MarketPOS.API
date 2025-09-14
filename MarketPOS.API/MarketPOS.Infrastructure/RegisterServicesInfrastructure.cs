using MarketPOS.Application.RepositoryInterfaces.RepositoryCategoryAndWareHouse;
using MarketPOS.Application.Services.InterfacesServices.EntityIntrerfaceService;
using MarketPOS.Application.Specifications;
using MarketPOS.Infrastructure.Repositories.RepositoryCategoryAndWareHouse;

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

                // ✅ إضافة retry على الفشل المؤقت
                sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,                               // عدد المحاولات قبل ما يرمي Exception
                    maxRetryDelay: TimeSpan.FromSeconds(10),        // أقصى تأخير بين المحاولات
                    errorNumbersToAdd: null                         // ممكن تحدد أكواد أخطاء معينة، null يعني الكل
                );
            });
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
        services.AddScoped<IActivelngredinentsRepo, ActiveingredinentRepository>();
        services.AddScoped<IWareHouseRepo, WareHouseRepository>();
        services.AddScoped<ISupplierRepo, SupplierRepo>();

        //✅ تسجيل Services
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
