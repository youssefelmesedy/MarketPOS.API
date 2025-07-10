namespace MarketPOS.Design.FactoryResult;
public class ResultFactory<THandler> : IResultFactory<THandler>
{
    private readonly IStringLocalizer<THandler>? _localizer;

    public ResultFactory(IStringLocalizer<THandler>? localizer = null)
    {
        _localizer = localizer;
    }

    public ResultDto<T> Success<T>(T data, string messageKey)
    {
        var message = _localizer?[messageKey] ?? messageKey;
        return ResultDto<T>.Ok(message, data);
    }

    public ResultDto<T> Fail<T>(string messageKey)
    {
        var message = _localizer?[messageKey] ?? messageKey;
        return ResultDto<T>.Fail(message);
    }
}

