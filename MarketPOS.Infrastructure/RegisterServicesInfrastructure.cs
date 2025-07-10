using Market.POS.Application.Services.Interfaces;
using Market.POS.Infrastructure.Repositories;
using Market.POS.Infrastructure.Services;
using MarketPOS.Application.Common.Interfaces;
using MarketPOS.Application.Common.Interfaces.ProductRepositorys;
using MarketPOS.Application.Common.Interfaces.RepositoryCategory;
using MarketPOS.Application.Common.Interfaces.RepositorySupplier;
using MarketPOS.Application.Services.Interfaces;
using MarketPOS.Infrastructure.Context;
using MarketPOS.Infrastructure.Repositories.ProductRepositorys;
using MarketPOS.Infrastructure.Repositories.RepositoryCategory;
using MarketPOS.Infrastructure.Repositories.RepositorySupplier;
using MarketPOS.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
