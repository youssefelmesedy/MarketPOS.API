namespace MarketPOS.Application.Common.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string name)
        : base($"{name} was not found.") { }

    public NotFoundException(string name, object key)
        : base($"{name} with key '{key}' was not found.") { }
}

