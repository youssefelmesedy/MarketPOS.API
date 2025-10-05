using Market.Domain.Entities.Auth;
using MarketPOS.Application.Services.InterfacesServices.FileStorage;
using MarketPOS.Application.Services.InterfacesServices.InterFacesAuthentication;
using MarketPOS.Infrastructure.Services.Authentication.AuthenticationService;
using MarketPOS.Infrastructure.Services.Authentication.JWTServices;
using MarketPOS.Infrastructure.Services.FileStorage;
using Microsoft.AspNetCore.Identity;


namespace MarketPOS.Infrastructure;
public static class RegisterServicesInfrastructure
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        var connectionString = environment == "Development"
                 ? configuration.GetConnectionString("LocalConnection")
                 : configuration.GetConnectionString("ServerConnection");

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(connectionString, sqlOptions =>
            {
                sqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);

                sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,                       
                    maxRetryDelay: TimeSpan.FromSeconds(10),
                    errorNumbersToAdd: null                 
                );
            });
        });

        // 2. Identity
        services.AddIdentity<User, IdentityRole<Guid>>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredLength = 6;
        })
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();

        // JWT Service
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IRefreshTokenService, RefreshTokenService>();
        services.AddScoped<IFileService, FileService>();
        services.AddScoped<IAuthService, AuthService>();

        // Debandancey Injection for UnitOfWork and Repositories
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Debandancey Injection for Generic Repositories and Decorate Caching
        services.AddScoped(typeof(IFullRepository<>), typeof(GenericRepository<>));
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

        Console.WriteLine($"EnviromentName: {environment}");

        return services;
    }
}
