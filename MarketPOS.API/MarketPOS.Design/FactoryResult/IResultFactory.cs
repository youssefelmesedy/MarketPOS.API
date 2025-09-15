namespace MarketPOS.Design.FactoryResult;
public interface IResultFactory<THandler>
{
    ResultDto<T> Success<T>(T data, string messageKey);
    ResultDto<T> Fail<T>(string messageKey, object? error = null);
}

