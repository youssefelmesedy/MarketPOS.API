using MarketPOS.Design.FactoryResult;
using MarketPOS.Design.FactoryServices;
using Microsoft.Extensions.DependencyInjection;

namespace MarketPOS.Design;

public static class RegisterDesignPattern
{
    public static IServiceCollection AddDesignPatternServices(this IServiceCollection service)
    {

        service.AddScoped(typeof(IResultFactory<>),typeof(ResultFactory<>));
        service.AddScoped<IServiceFactory, ServiceFactory>();
        return service;
    }
}
