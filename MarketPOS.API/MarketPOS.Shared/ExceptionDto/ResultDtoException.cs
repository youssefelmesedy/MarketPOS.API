namespace MarketPOS.Shared.ExceptionDto;
[Serializable]
public sealed class ResultDtoException : Exception
{
    public int StatusCode { get; init; } = 400;
    public string? ErrorCode { get; init; }
}

