namespace MarketPOS.Application.Common.Helpers.LocalizationPostProcessorMappeing;
public interface ILocalizationPostProcessor
{
    T Apply<T>(T model) where T : class;
    IEnumerable<T> Apply<T>(IEnumerable<T> list) where T : class;
    List<T> Apply<T>(List<T> list) where T : class;
}

