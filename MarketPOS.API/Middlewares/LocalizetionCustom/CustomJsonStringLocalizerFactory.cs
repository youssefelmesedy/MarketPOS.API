using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Localization;

namespace MarketPOS.API.Middlewares.LocalizetionCustom;
public class CustomJsonStringLocalizerFactory : IStringLocalizerFactory
{
    public IStringLocalizer Create(Type resourceSource)
    {
        return new JsonStringLocalizer();
    }

    public IStringLocalizer Create(string baseName, string location)
    {
        return new JsonStringLocalizer();
    }
}
