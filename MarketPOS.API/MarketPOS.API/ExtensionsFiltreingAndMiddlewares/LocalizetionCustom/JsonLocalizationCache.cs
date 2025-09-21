using System.Collections.Concurrent;

namespace MarketPOS.API.Middlewares.LocalizetionCustom;
public class JsonLocalizationCache
{
    public ConcurrentDictionary<string, Dictionary<string, string>> Cache { get; } = new();

    public void Clear() => Cache.Clear();
}
