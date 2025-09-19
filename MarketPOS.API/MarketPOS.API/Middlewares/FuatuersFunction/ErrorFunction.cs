namespace MarketPOS.API.Middlewares.FeaturesFunction;

public static class ErrorFunction
{
    private static ObjectResult CreateErrorResult(
        int statusCode,
        bool isSuccess = false,
        string? message = null,
        object? errors = null)
    {
        return new ObjectResult(new ResultDto<object>
        {
            IsSuccess = isSuccess,
            Message = message,
            Errors = errors
        })
        {
            StatusCode = statusCode
        };
    }

    public static NotFoundObjectResult NotFound(bool isSuccess = false, string? message = null, object? errors = null)
        => new(CreateErrorResult(StatusCodes.Status404NotFound, isSuccess, message, errors).Value!);

    public static BadRequestObjectResult BadRequest(bool isSuccess = false, string? message = null, object? errors = null)
        => new(CreateErrorResult(StatusCodes.Status400BadRequest, isSuccess, message, errors).Value!);

    public static ConflictObjectResult ConflictRequest(bool isSuccess = false, string? message = null, object? errors = null)
        => new(CreateErrorResult(StatusCodes.Status409Conflict, isSuccess, message, errors).Value!);
}
