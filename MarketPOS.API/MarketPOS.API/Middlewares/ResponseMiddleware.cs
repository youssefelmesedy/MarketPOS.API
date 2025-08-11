using System.Globalization;
namespace MarketPOS.API.Middlewares;
public class ResponseMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ResponseMiddleware> _logger;
    private readonly IStringLocalizer<ResponseMiddleware> _localizer;

    public ResponseMiddleware(RequestDelegate next, ILogger<ResponseMiddleware> logger, IStringLocalizer<ResponseMiddleware> localizer)
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

            if (string.IsNullOrWhiteSpace(bodyText) ||
                !context.Response.ContentType.Contains("application/json"))
            {
                await context.Response.WriteAsync(bodyText ?? "");
                return;
            }

            using var jsonDoc = JsonDocument.Parse(bodyText);
            var root = jsonDoc.RootElement;

            if (root.TryGetProperty("isSuccess", out _) &&
                root.TryGetProperty("message", out var messageElement))
            {
                var isSuccess = root.GetProperty("isSuccess").GetBoolean();
                var message = messageElement.GetString() ?? "";

                object? data = null;
                if (root.TryGetProperty("data", out var dataElement))
                    data = JsonSerializer.Deserialize<object>(dataElement.GetRawText());

                object? errors = null;
                if (root.TryGetProperty("errors", out var errorsElement))
                    errors = JsonSerializer.Deserialize<object>(errorsElement.GetRawText());

                var final = new ApiResponse<object>
                {
                    Success = isSuccess,
                    Message = message,
                    Data =  data,
                    Errors = errors
                };

                var json = JsonSerializer.Serialize(final);
                await context.Response.WriteAsync(json);
                return;
            }

            var statusCode = context.Response.StatusCode;
            var parsed = JsonSerializer.Deserialize<object>(bodyText);

            var response = new ApiResponse<object>
            {
                Success = statusCode is >= 200 and < 300,
                Message = statusCode switch
                {
                    200 => _localizer["Success"],
                    201 => _localizer["Created"],
                    400 => bodyText.Contains("errors") ? _localizer["ValidationFailed"] : _localizer["BadRequest"],
                    404 => _localizer["NotFound"],
                    500 => _localizer["InternalError"],
                    _ => _localizer["Success"]
                },

                Data = statusCode is >= 200 and < 300 ? parsed : null,
                Errors = statusCode >= 400 ? parsed : null
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed in ResponseMiddleware");

            if (!context.Response.HasStarted && context.Response.Body.CanWrite)
            {
                context.Response.Body = originalBody;
                context.Response.ContentType = "application/json";
                context.Response.Headers["Content-Language"] = CultureInfo.CurrentUICulture.Name;

                var fallback = JsonSerializer.Serialize(new
                {
                    success = false,
                    message = "Unhandled response middleware error.",
                    details = ex.Message
                });

                await context.Response.WriteAsync(fallback);
            }
        }
    }
}