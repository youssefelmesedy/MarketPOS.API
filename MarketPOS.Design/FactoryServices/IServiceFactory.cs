namespace MarketPOS.Design.FactoryServices;
public interface IServiceFactory
{
    TService GetService<TService>();
}
