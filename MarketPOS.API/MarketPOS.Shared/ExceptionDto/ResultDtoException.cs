namespace MarketPOS.Shared.ExceptionDto;
[Serializable]
public sealed class ResultDtoException : Exception
{
    public int StatusCode { get; init; } = 400;
    public string? ErrorCode { get; init; }

    public ResultDtoException(string message, int statusCode = 400, string? errorCode = null)
        : base(message)
    {
        StatusCode = statusCode;
        ErrorCode = errorCode;
    }

    public ResultDtoException(string message, Exception? innerException, int statusCode = 400, string? errorCode = null)
        : base(message, innerException)
    {
        StatusCode = statusCode;
        ErrorCode = errorCode;
    }

    protected ResultDtoException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
        StatusCode = info.GetInt32(nameof(StatusCode));
        ErrorCode = info.GetString(nameof(ErrorCode));
    }

    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        base.GetObjectData(info, context);
        info.AddValue(nameof(StatusCode), StatusCode);
        info.AddValue(nameof(ErrorCode), ErrorCode);
    }
}

