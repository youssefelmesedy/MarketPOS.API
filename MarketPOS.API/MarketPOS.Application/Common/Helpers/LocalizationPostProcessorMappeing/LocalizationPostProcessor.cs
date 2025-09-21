using System.Collections;

namespace MarketPOS.Application.Common.Helpers.LocalizationPostProcessorMappeing;
public class LocalizationPostProcessor : ILocalizationPostProcessor
{
    private readonly IStringLocalizer<LocalizationPostProcessor> _localizer;

    public LocalizationPostProcessor(IStringLocalizer<LocalizationPostProcessor> localizer)
    {
        _localizer = localizer;
    }

    public T Apply<T>(T model) where T : class
    {
        if (model == null) return null!;

        var visited = new HashSet<object>();
        ProcessObject(model, visited);

        return model;
    }

    public IEnumerable<T> Apply<T>(IEnumerable<T> list) where T : class
    {
        if (list == null) return Enumerable.Empty<T>();

        var visited = new HashSet<object>();
        foreach (var item in list)
        {
            ProcessObject(item, visited);
        }

        return list;
    }

    public List<T> Apply<T>(List<T> list) where T : class
    {
        if (list == null) return new List<T>();

        var visited = new HashSet<object>();
        foreach (var item in list)
        {
            ProcessObject(item, visited);
        }

        return list;
    }

    private void ProcessObject(object obj, HashSet<object> visited)
    {
        if (obj == null || visited.Contains(obj))
            return;

        visited.Add(obj);

        var properties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.CanRead && p.CanWrite);

        foreach (var prop in properties)
        {
            var propType = prop.PropertyType;

            if (propType == typeof(string))
            {
                var value = prop.GetValue(obj) as string;
                if (string.IsNullOrWhiteSpace(value))
                {
                    prop.SetValue(obj, _localizer["NoValues"].Value);
                }
            }
            else if (typeof(IEnumerable).IsAssignableFrom(propType) && propType != typeof(string))
            {
                var enumerable = prop.GetValue(obj) as IEnumerable;
                if (enumerable != null)
                {
                    foreach (var item in enumerable)
                    {
                        ProcessObject(item!, visited);
                    }
                }
            }
            else if (!propType.IsPrimitive && !propType.IsEnum && propType != typeof(DateTime) && !propType.IsValueType)
            {
                var nestedObject = prop.GetValue(obj);
                if (nestedObject != null)
                {
                    ProcessObject(nestedObject, visited);
                }
            }
        }
    }
}



