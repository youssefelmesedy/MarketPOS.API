using MarketPOS.API.Bootstrapper.Extensions.ExtensionCacheing;
using MarketPOS.Design;
using MarketPOS.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MarketPOS.API.Bootstrapper
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddMarketPOS(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddApplicationServices()
                .AddInfrastructureServices(configuration)
                .AddDesignPatternServices()
                .AddCustomCaching(configuration)
                .AddCustomLocalization()
                .AddCustomValidation()
                .AddCustomSwagger()
                .AddCustomAuthenticationAndAuthorization(configuration);

            return services;
        }
    }
}
