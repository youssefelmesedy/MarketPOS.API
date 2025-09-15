using System.Globalization;

namespace MarketPOS.API.Middlewares;

#region ResponseMiddleware New
public class ResponseMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ResponseMiddleware> _logger;
    private readonly IStringLocalizer<ResponseMiddleware> _localizer;

    public ResponseMiddleware(RequestDelegate next,
                              ILogger<ResponseMiddleware> logger,
                              IStringLocalizer<ResponseMiddleware> localizer)
    {
        _next = next;
        _logger = logger;
        _localizer = localizer;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var originalBody = context.Response.Body;
        await using var memStream = new MemoryStream();
        context.Response.Body = memStream;

        try
        {
            await _next(context);

            memStream.Seek(0, SeekOrigin.Begin);
            var bodyText = await new StreamReader(memStream).ReadToEndAsync();

            context.Response.Body = originalBody;
            context.Response.ContentType = "application/json";
            context.Response.ContentLength = null;
            context.Response.Headers["Content-Language"] = CultureInfo.CurrentUICulture.Name;

            if (string.IsNullOrWhiteSpace(bodyText))
            {
                await context.Response.WriteAsync("");
                return;
            }

            // 1️⃣ لو Error (ProblemDetails)
            var errorResponse = TryHandleProblemDetails(bodyText);
            if (errorResponse is not null)
            {
                await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
                return;
            }

            // 2️⃣ لو Success أو Data عادية
            var successResponse = HandleNormalResponse(bodyText, context.Response.StatusCode);
            await context.Response.WriteAsync(JsonSerializer.Serialize(successResponse));
        }
        catch (Exception ex)
        {
            await HandleMiddlewareException(context, originalBody, ex);
        }
    }

    // 🔴 دالة خاصة للتعامل مع ProblemDetails
    private ApiResponse<object>? TryHandleProblemDetails(string bodyText)
    {
        try
        {
            var problem = JsonSerializer.Deserialize<ExtendedProblemDetails>(bodyText,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (problem is not null && problem.Status.HasValue)
            {
                return new ApiResponse<object>
                {
                    Success = false,
                    Status = problem.Status.Value,
                    Message = problem.Title,
                    Data = null,
                    Errors = new
                    {
                        problem.Detail,
                        problem.ErrorCode,
                        problem.ErrorSource,
                        problem.Errors
                    }
                };
            }
        }
        catch
        {
            // تجاهل لو مش ProblemDetails
        }

        return null;
    }

    // 🟢 دالة خاصة للتعامل مع الـ Responses العادية
    private ApiResponse<object> HandleNormalResponse(string bodyText, int statusCode)
    {
        using var jsonDoc = JsonDocument.Parse(bodyText);
        var root = jsonDoc.RootElement;

        bool successFlag = statusCode is >= 200 and < 300;

        if (root.TryGetProperty("isSuccess", out var isSuccessProp))
            successFlag = isSuccessProp.GetBoolean();

        return new ApiResponse<object>
        {
            Success = successFlag,
            Status = statusCode,
            Message = root.TryGetProperty("message", out var msgProp)
                        ? msgProp.GetString() ?? ""
                        : (successFlag ? _localizer["Success"] : _localizer["Error"]),
            Data = successFlag && root.TryGetProperty("data", out var dataProp)
                        ? JsonSerializer.Deserialize<object>(dataProp.GetRawText())
                        : null,
            Errors = !successFlag && root.TryGetProperty("errors", out var errProp)
                        ? JsonSerializer.Deserialize<object>(errProp.GetRawText())
                        : null
        };
    }

    // ⚠️ دالة خاصة للتعامل مع Exceptions جوة الميدل وير
    private async Task HandleMiddlewareException(HttpContext context, Stream originalBody, Exception ex)
    {
        _logger.LogError(ex, "Failed in ResponseMiddleware");

        if (!context.Response.HasStarted)
        {
            context.Response.Body = originalBody;
            context.Response.ContentType = "application/json";

            var fallback = ApiResponse<object>.Fail("Unhandled middleware error.", 500, new { ex.Message });
            await context.Response.WriteAsync(JsonSerializer.Serialize(fallback));
        }
    }
}

#endregion
#region ResponseMiddleware Old
//public class ResponseMiddleware
//{
//    private readonly RequestDelegate _next;
//    private readonly ILogger<ResponseMiddleware> _logger;
//    private readonly IStringLocalizer<ResponseMiddleware> _localizer;

//    public ResponseMiddleware(RequestDelegate next, ILogger<ResponseMiddleware> logger, IStringLocalizer<ResponseMiddleware> localizer)
//    {
//        _next = next;
//        _logger = logger;
//        _localizer = localizer;
//    }

//    public async Task InvokeAsync(HttpContext context)
//    {
//        var originalBody = context.Response.Body;
//        await using var memStream = new MemoryStream();
//        context.Response.Body = memStream;

//        try
//        {
//            await _next(context);

//            memStream.Seek(0, SeekOrigin.Begin);
//            var bodyText = await new StreamReader(memStream).ReadToEndAsync();

//            context.Response.Body = originalBody;
//            context.Response.ContentType = "application/json";
//            context.Response.ContentLength = null;
//            context.Response.Headers["Content-Language"] = CultureInfo.CurrentUICulture.Name;

//            if (string.IsNullOrWhiteSpace(bodyText) ||
//                !context.Response.ContentType.Contains("application/json"))
//            {
//                await context.Response.WriteAsync(bodyText ?? "");
//                return;
//            }

//            // 🆕 لو الرد عبارة عن ProblemDetails أو ValidationProblemDetails
//            try
//            {
//                var jsonProblem = JsonSerializer.Deserialize<ProblemDetails>(bodyText,
//                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

//                if (jsonProblem is not null && !string.IsNullOrEmpty(jsonProblem.Title))
//                {
//                    object errorsObj = null!;

//                    // 🆕 لو فيه validation errors
//                    if (jsonProblem is ValidationProblemDetails vpd)
//                        errorsObj = vpd.Errors;

//                    var unifiedError = new ApiResponse<object>
//                    {
//                        Success = false,
//                        Status = jsonProblem.Status ?? context.Response.StatusCode,
//                        Message = jsonProblem.Title,
//                        Data = null,
//                        Errors = errorsObj ?? jsonProblem.Extensions
//                    };

//                    var errorJson = JsonSerializer.Serialize(unifiedError);
//                    await context.Response.WriteAsync(errorJson);
//                    return;
//                }
//            }
//            catch
//            {
//                // تجاهل لو مش ProblemDetails
//            }

//            using var jsonDoc = JsonDocument.Parse(bodyText);
//            var root = jsonDoc.RootElement;
//            var statusCode = context.Response.StatusCode;

//            if (root.TryGetProperty("isSuccess", out _) &&
//                root.TryGetProperty("message", out var messageElement))
//            {
//                var isSuccess = root.GetProperty("isSuccess").GetBoolean();
//                var message = messageElement.GetString() ?? "";

//                object? data = null;
//                if (root.TryGetProperty("data", out var dataElement))
//                    data = JsonSerializer.Deserialize<object>(dataElement.GetRawText());

//                object? errors = null;
//                if (root.TryGetProperty("errors", out var errorsElement))
//                    errors = JsonSerializer.Deserialize<object>(errorsElement.GetRawText());

//                var final = new ApiResponse<object>
//                {
//                    Success = isSuccess,
//                    Status = statusCode,
//                    Message = message,
//                    Data = data,
//                    Errors = errors
//                };

//                var json = JsonSerializer.Serialize(final);
//                await context.Response.WriteAsync(json);
//                return;
//            }

//            var parsed = JsonSerializer.Deserialize<object>(bodyText);

//            var response = new ApiResponse<object>
//            {
//                Success = statusCode is >= 200 and < 300,
//                Message = statusCode switch
//                {
//                    200 => _localizer["Success"],
//                    201 => _localizer["Created"],
//                    400 => bodyText.Contains("errors") ? _localizer["ValidationFailed"] : _localizer["BadRequest"],
//                    404 => _localizer["NotFound"],
//                    500 => _localizer["InternalError"],
//                    _ => _localizer["Success"]
//                },

//                Data = statusCode is >= 200 and < 300 ? parsed : null,
//                Errors = statusCode >= 400 ? parsed : null
//            };

//            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
//        }
//        catch (Exception ex)
//        {
//            _logger.LogError(ex, "Failed in ResponseMiddleware");

//            if (!context.Response.HasStarted && context.Response.Body.CanWrite)
//            {
//                context.Response.Body = originalBody;
//                context.Response.ContentType = "application/json";
//                context.Response.Headers["Content-Language"] = CultureInfo.CurrentUICulture.Name;

//                var fallback = JsonSerializer.Serialize(new
//                {
//                    success = false,
//                    message = "Unhandled response middleware error.",
//                    details = ex.Message
//                });

//                await context.Response.WriteAsync(fallback);
//            }
//        }
//    }

//}
#endregion

