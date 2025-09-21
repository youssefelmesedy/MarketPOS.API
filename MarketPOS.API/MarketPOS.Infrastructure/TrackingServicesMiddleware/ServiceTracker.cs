using System.Collections.Concurrent;

namespace MarketPOS.Infrastructure.TrackingServicesMiddleware;

public static class ServiceTracker
{
    private static readonly ConcurrentDictionary<string, int> _resolvedServices = new();

    public static void Add(string serviceName)
    {
        _resolvedServices.AddOrUpdate(serviceName, 1, (_, count) => count + 1);
    }

    public static IReadOnlyDictionary<string, int> GetResolvedServices()
    {
        return _resolvedServices;
    }

    public static void Clear()
    {
        _resolvedServices.Clear();
    }
}


