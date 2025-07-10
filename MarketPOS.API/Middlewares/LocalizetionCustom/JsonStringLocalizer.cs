using Newtonsoft.Json;

namespace MarketPOS.API.Middlewares.LocalizetionCustom;
public class JsonStringLocalizer : IStringLocalizer
{
    private readonly Newtonsoft.Json.JsonSerializer _jsonSerializer = new();

    public LocalizedString this[string name]
    {
        get
        {
            var value = GetString(name);
            return new LocalizedString(name, value);
        }
    }

    public LocalizedString this[string name, params object[] arguments] 
    {
        get
        {
             var actualValue = this[name];
            return !actualValue.ResourceNotFound 
                   ? new LocalizedString(name, string.Format(actualValue.Value, arguments)) 
                   : actualValue;
        }
    }

    public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
    {
        var filePath = $"Resources/{Thread.CurrentThread.CurrentCulture.Name}.json";

        using FileStream FileStream = new(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);

        using StreamReader StreamReader = new(FileStream);

        using JsonTextReader reader = new(StreamReader);

        while (reader.Read())
        {
            if (reader.TokenType != JsonToken.PropertyName)
                continue;

            var key = reader.Value as string;
            reader.Read(); // Move to the value token
            var value = _jsonSerializer.Deserialize<string>(reader);
            yield return new LocalizedString(key, value ?? string.Empty);
        }
    }

    private string GetValueFromeJson(string propertyName, string filePath)
    {
        if (string.IsNullOrEmpty(propertyName) || string.IsNullOrEmpty(filePath))
            return string.Empty;

        using FileStream FileStream = new (filePath, FileMode.Open, FileAccess.Read, FileShare.Read);

        using StreamReader StreamReader = new (FileStream);

        using JsonTextReader reader = new (StreamReader);

        while (reader.Read())
        {
            if(reader.TokenType == JsonToken.PropertyName && reader.Value as string == propertyName)
            {
                reader.Read(); // Move to the value token
                return _jsonSerializer.Deserialize<string>(reader)!;
            }
        }

        return string.Empty;
    }

    private string GetString(string key)
    {
        var filePath =$"Resources/{Thread.CurrentThread.CurrentCulture.Name}.json";
        var fullFilePath = Path.GetFullPath(filePath);

        if (File.Exists(fullFilePath))
        {
            var result = GetValueFromeJson(key, fullFilePath);
            return result;
        }

        return string.Empty;
    }
}
