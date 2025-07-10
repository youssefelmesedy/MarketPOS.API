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

        var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.PropertyType == typeof(string));

        foreach (var prop in props)
        {
            var value = prop.GetValue(model) as string;
            if (string.IsNullOrWhiteSpace(value))
            {
                prop.SetValue(model, _localizer["NoValues"].Value);
            }
        }

        return model;
    }

    public IEnumerable<T> Apply<T>(IEnumerable<T> list) where T : class
    {
        foreach (var item in list)
            Apply(item);

        return list;
    }
}


