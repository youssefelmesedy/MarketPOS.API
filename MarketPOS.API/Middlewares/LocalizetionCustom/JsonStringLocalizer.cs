using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Concurrent;
using System.Globalization;
namespace MarketPOS.API.Middlewares.LocalizetionCustom;

#region the Implementation Localized Json Not Use ConcurrentDictionary
//public class JsonStringLocalizer : IStringLocalizer
//{
//    private readonly Newtonsoft.Json.JsonSerializer _jsonSerializer = new();

//    public LocalizedString this[string name]
//    {
//        get
//        {
//            var value = GetString(name);
//            return new LocalizedString(name, value);
//        }
//    }

//    public LocalizedString this[string name, params object[] arguments] 
//    {
//        get
//        {
//             var actualValue = this[name];
//            return !actualValue.ResourceNotFound 
//                   ? new LocalizedString(name, string.Format(actualValue.Value, arguments)) 
//                   : actualValue;
//        }
//    }

//    public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
//    {
//        var filePath = $"Resources/{Thread.CurrentThread.CurrentCulture.Name}.json";

//        using FileStream FileStream = new(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);

//        using StreamReader StreamReader = new(FileStream);

//        using JsonTextReader reader = new(StreamReader);

//        while (reader.Read())
//        {
//            if (reader.TokenType != JsonToken.PropertyName)
//                continue;

//            var key = reader.Value as string;
//            reader.Read(); // Move to the value token
//            var value = _jsonSerializer.Deserialize<string>(reader);
//            yield return new LocalizedString(key, value ?? string.Empty);
//        }
//    }

//    private string GetValueFromeJson(string propertyName, string filePath)
//    {
//        if (string.IsNullOrEmpty(propertyName) || string.IsNullOrEmpty(filePath))
//            return string.Empty;

//        using FileStream FileStream = new (filePath, FileMode.Open, FileAccess.Read, FileShare.Read);

//        using StreamReader StreamReader = new (FileStream);

//        using JsonTextReader reader = new (StreamReader);

//        while (reader.Read())
//        {
//            if(reader.TokenType == JsonToken.PropertyName && reader.Value as string == propertyName)
//            {
//                reader.Read(); // Move to the value token
//                return _jsonSerializer.Deserialize<string>(reader)!;
//            }
//        }

//        return string.Empty;
//    }

//    private string GetString(string key)
//    {
//        var filePath =$"Resources/{Thread.CurrentThread.CurrentCulture.Name}.json";
//        var fullFilePath = Path.GetFullPath(filePath);

//        if (File.Exists(fullFilePath))
//        {
//            var result = GetValueFromeJson(key, fullFilePath);
//            return result;
//        }

//        return string.Empty;
//    }
//}
#endregion

#region the Implementation Localized Json Use ConcurrentDictionary and Clear Cache
public class JsonStringLocalizer : IStringLocalizer
{
    private readonly string _resourcesPath;
    private readonly JsonLocalizationCache _cache = new();
    private readonly Newtonsoft.Json.JsonSerializer _jsonSerializer = new();

    public JsonStringLocalizer(JsonLocalizationCache cache, string resourcesPath = "Resources")
    {
        _resourcesPath = resourcesPath;
        _cache = cache ?? throw new ArgumentNullException(nameof(cache));
    }

    public LocalizedString this[string name]
    {
        get
        {
            var value = GetString(name);
            return new LocalizedString(name, value ?? name, resourceNotFound: string.IsNullOrEmpty(value));
        }
    }

    public LocalizedString this[string name, params object[] arguments]
    {
        get
        {
            var actual = this[name];
            return !actual.ResourceNotFound
                ? new LocalizedString(name, string.Format(actual.Value, arguments))
                : actual;
        }
    }

    public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
    {
        var dict = GetCurrentCultureDictionary();

        foreach (var pair in dict)
        {
            yield return new LocalizedString(pair.Key, pair.Value, resourceNotFound: false);
        }
    }

    private Dictionary<string, string> GetCurrentCultureDictionary()
    {
        var culture = CultureInfo.CurrentCulture.Name;
        return _cache.Cache.GetOrAdd(culture, LoadJsonFile);
    }

    private Dictionary<string, string> LoadJsonFile(string culture)
    {
        var filePath = Path.Combine(_resourcesPath, $"{culture}.json");

        if (!File.Exists(filePath))
            return new Dictionary<string, string>();

        using var stream = File.OpenRead(filePath);
        using var reader = new StreamReader(stream);
        using var jsonReader = new JsonTextReader(reader);

        var jObject = _jsonSerializer.Deserialize<JObject>(jsonReader);

        return jObject?.Properties()
            .ToDictionary(p => p.Name, p => p.Value?.ToString() ?? string.Empty)
            ?? new Dictionary<string, string>();
    }

    private string? GetString(string key)
    {
        var dict = GetCurrentCultureDictionary();
        return dict.TryGetValue(key, out var value) ? value : null;
    }
}
#endregion
