namespace MarketPOS.API.Middlewares.LocalizetionCustom;

#region the Implementation Localized Json Not Use ConcurrentDictionary and Clear Cache
//public class CustomJsonStringLocalizerFactory : IStringLocalizerFactory
//{
//    public IStringLocalizer Create(Type resourceSource)
//    {
//        return new JsonStringLocalizer();
//    }

//    public IStringLocalizer Create(string baseName, string location)
//    {
//        return new JsonStringLocalizer();
//    }
//}
#endregion

#region the Implementation Localized Json Use ConcurrentDictionary and Clear Cache
public class CustomJsonStringLocalizerFactory : IStringLocalizerFactory
{
    private readonly JsonLocalizationCache _cache;
    private readonly string _resourcesPath;

    public CustomJsonStringLocalizerFactory(JsonLocalizationCache cache = null!, string resourcesPath = "Resources")
    {
        _cache = cache ?? throw new ArgumentNullException(nameof(_cache), "Not Found Cacheing Memorey");
        _resourcesPath = resourcesPath;
    }

    public IStringLocalizer Create(Type resourceSource)
    {
        return new JsonStringLocalizer(_cache, _resourcesPath);
    }

    public IStringLocalizer Create(string baseName, string location)
    {
        return new JsonStringLocalizer(_cache, _resourcesPath);
    }
}
#endregion