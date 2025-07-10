

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
        if (typeof(TService) == null)
        {
            _logger.LogWarning("Attempted to resolve a null service type.");
            throw new ArgumentNullException(nameof(TService), "Service type cannot be null.");
        }

        try
        {
                return _provider.GetRequiredService<TService>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to resolve service of type {ServiceType}.", typeof(TService).Name);
            throw new InvalidOperationException($"Unable to resolve service of type {typeof(TService).Name}.", ex);
        }
    }
}
