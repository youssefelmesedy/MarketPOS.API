namespace MarketPOS.API.Middlewares.Exceptions;
public class ResultDtoException : Exception
{
    public ResultDtoException() { }

    public ResultDtoException(string message)
        : base(message) { }

    public ResultDtoException(string message, Exception innerException)
        : base(message, innerException) { }
}
