namespace MarketPOS.Design.FactoryServices;
public class ServiceFactory : IServiceFactory
{
    private readonly IServiceProvider _provider;
    private readonly ILogger<ServiceFactory> _logger;

    public ServiceFactory(IServiceProvider provider, ILogger<ServiceFactory> logger)
    {
        _provider = provider;
        _logger = logger;
    }

    public TService GetService<TService>()
    {
        try
        {
            var service = _provider.GetService<TService>();
            if (service == null)
            {
                _logger.LogWarning("Service of type {ServiceType} not found.", typeof(TService).Name);
                throw new InvalidOperationException($"Service of type {typeof(TService).Name} is not registered.");
            }
            return service;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to resolve service of type {ServiceType}.", typeof(TService).Name);
            throw new InvalidOperationException($"Unable to resolve service of type {typeof(TService).Name}.", ex);
        }
    }
}
