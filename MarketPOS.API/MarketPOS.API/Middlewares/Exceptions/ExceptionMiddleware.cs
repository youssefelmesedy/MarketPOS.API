using AutoMapper;
using MarketPOS.Application.Common.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using YourProjectNamespace.Application.Common.Exceptions;

namespace MarketPOS.Infrastructure.Exceptions;

#region exception middleware old
//public class ExceptionMiddleware
//{
//    private readonly RequestDelegate _next;
//    private readonly ILogger<ExceptionMiddleware> _logger;
//    private readonly IHostEnvironment _env;
//    private readonly IStringLocalizer<ExceptionMiddleware> _localizer;

//    public ExceptionMiddleware(
//        RequestDelegate next,
//        ILogger<ExceptionMiddleware> logger,
//        IHostEnvironment env,
//        IStringLocalizer<ExceptionMiddleware> localizer)
//    {
//        _next = next;
//        _logger = logger;
//        _env = env;
//        _localizer = localizer;
//    }

//    public async Task InvokeAsync(HttpContext context)
//    {
//        try
//        {
//            await _next(context);
//        }
//        catch (Exception ex)
//        {
//            _logger.LogError(ex, _localizer["InternalError"]);
//            await HandleExceptionAsync(context, ex);
//        }
//    }

//    private async Task HandleExceptionAsync(HttpContext context, Exception ex)
//    {
//        context.Response.ContentType = "application/json";

//        // 1. FluentValidation Errors
//        if (ex is ValidationException)
//        {
//            context.Response.StatusCode = StatusCodes.Status400BadRequest;

//            var problem = new ExtendedProblemDetails
//            {
//                Title = _localizer["ValidationFailed"],
//                Detail = _localizer["Please review the input data."],
//                Status = StatusCodes.Status400BadRequest,
//                Instance = context.Request.Path,
//                Type = "https://httpstatuses.com/400",
//                ErrorCode = "ValidationError",
//                ErrorSource = ex.Source
//            };

//            await context.Response.WriteAsync(Json(problem));
//            return;
//        }

//        // 2. NotFoundException
//        if (ex is NotFoundException notFoundEx)
//        {
//            context.Response.StatusCode = StatusCodes.Status404NotFound;

//            var problem = new ExtendedProblemDetails
//            {
//                Title = _localizer["NotFound"],
//                Detail = _localizer[notFoundEx.Message],
//                Status = StatusCodes.Status404NotFound,
//                Instance = context.Request.Path,
//                Type = "https://httpstatuses.com/404",
//                ErrorCode = "NotFound",
//                ErrorSource = notFoundEx.Source
//            };

//            await context.Response.WriteAsync(Json(problem));
//            return;
//        }

//        // 3. BusinessException (custom logic)
//        if (ex is BusinessException businessEx)
//        {
//            context.Response.StatusCode = StatusCodes.Status400BadRequest;

//            var problem = new ExtendedProblemDetails
//            {
//                Title = _localizer["BusinessError"],
//                Detail = businessEx.Message, // هنا بتظهر الرسالة اللي انت بعتها
//                Status = StatusCodes.Status400BadRequest,
//                Instance = context.Request.Path,
//                Type = "https://httpstatuses.com/400",
//                ErrorCode = "BusinessException",
//                ErrorSource = ex.Source
//            };

//            await context.Response.WriteAsync(Json(problem));
//            return;
//        }

//        // 4. Invalid JSON
//        if (ex is JsonException jsonEx)
//        {
//            context.Response.StatusCode = StatusCodes.Status400BadRequest;

//            var errors = new Dictionary<string, string[]>();
//            var match = Regex.Match(jsonEx.Message, @"property '(.+?)'");

//            if (match.Success)
//            {
//                errors["$." + match.Groups[1].Value] = new[]
//                {
//                    _localizer["The JSON property '{0}' could not be mapped to any .NET member.", match.Groups[1].Value].Value
//                };
//            }
//            else
//            {
//                errors["json"] = new[] { jsonEx.Message };
//            }

//            var problem = new ExtendedProblemDetails
//            {
//                Title = _localizer["InvalidJson"],
//                Detail = _localizer["The request body contains invalid or unexpected properties."],
//                Status = StatusCodes.Status400BadRequest,
//                Instance = context.Request.Path,
//                Type = "https://httpstatuses.com/400",
//                ErrorCode = "JsonParsingError",
//                ErrorSource = ex.Source,
//                Errors = errors
//            };

//            await context.Response.WriteAsync(Json(problem));
//            return;
//        }

//        // 5. DbUpdateException (FK violation)
//        if (ex is DbUpdateException dbEx && dbEx.InnerException is SqlException sqlEx && sqlEx.Number == 547)
//        {
//            context.Response.StatusCode = StatusCodes.Status400BadRequest;

//            var problem = new ExtendedProblemDetails
//            {
//                Title = _localizer["InvalidReference"],
//                Detail = _localizer["One or more related entities do not exist in the database."],
//                Status = StatusCodes.Status400BadRequest,
//                Instance = context.Request.Path,
//                Type = "https://httpstatuses.com/400",
//                ErrorCode = "ForeignKeyConstraintViolation",
//                ErrorSource = ex.Source
//            };

//            await context.Response.WriteAsync(Json(problem));
//            return;
//        }

//        // 6. Default Exception Handler
//        var statusCode = ex switch
//        {
//            UnauthorizedAccessException => StatusCodes.Status403Forbidden,
//            ArgumentNullException => StatusCodes.Status400BadRequest,
//            ArgumentException => StatusCodes.Status400BadRequest,
//            DbUpdateException => StatusCodes.Status500InternalServerError,
//            TaskCanceledException => StatusCodes.Status408RequestTimeout,
//            TimeoutException => StatusCodes.Status408RequestTimeout,
//            AutoMapperMappingException => StatusCodes.Status500InternalServerError,
//            InvalidOperationException => StatusCodes.Status500InternalServerError,
//            _ => StatusCodes.Status500InternalServerError
//        };

//        context.Response.StatusCode = statusCode;

//        var defaultProblem = new ExtendedProblemDetails
//        {
//            Status = statusCode,
//            Instance = context.Request.Path,
//            Type = $"https://httpstatuses.com/{statusCode}",
//            Title = _localizer[ex.GetType().Name],
//            Detail = _env.IsDevelopment() ? ex.ToString() : _localizer["Something went wrong."],
//            ErrorSource = ex.Source
//        };

//        // AutoMapper special handling
//        if (ex is AutoMapperMappingException mapEx)
//        {
//            defaultProblem.Title = _localizer["MappingError"];
//            defaultProblem.ErrorCode = mapEx.InnerException?.GetType().Name;
//            defaultProblem.Detail = mapEx.InnerException?.Message ?? _localizer["Mapping failed."];
//            defaultProblem.ErrorSource = mapEx.MemberMap?.DestinationName ?? ex.Source;
//        }

//        await context.Response.WriteAsync(Json(defaultProblem));
//    }

//    private static string Json(object obj) =>
//        JsonSerializer.Serialize(obj, new JsonSerializerOptions
//        {
//            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
//            WriteIndented = true
//        });
//}
#endregion
#region exception middleware new
public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly IStringLocalizer<ExceptionMiddleware> _localizer;
    private readonly IHostEnvironment _env;

    public ExceptionMiddleware(RequestDelegate next,
        ILogger<ExceptionMiddleware> logger,
        IHostEnvironment env,
        IStringLocalizer<ExceptionMiddleware> localizer)
    {
        _next = next;
        _logger = logger;
        _env = env;
        _localizer = localizer;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        context.Response.ContentType = "application/json";

        var statusCode = ex switch
        {
            ValidationException => StatusCodes.Status400BadRequest,
            NotFoundException => StatusCodes.Status404NotFound,
            BusinessException => StatusCodes.Status400BadRequest,
            JsonException => StatusCodes.Status400BadRequest,
            DbUpdateException => StatusCodes.Status400BadRequest,
            UnauthorizedAccessException => StatusCodes.Status403Forbidden,
            ArgumentNullException => StatusCodes.Status400BadRequest,
            ArgumentException => StatusCodes.Status400BadRequest,
            TaskCanceledException => StatusCodes.Status408RequestTimeout,
            TimeoutException => StatusCodes.Status408RequestTimeout,
            AutoMapperMappingException => StatusCodes.Status500InternalServerError,
            InvalidOperationException => StatusCodes.Status500InternalServerError,
            _ => StatusCodes.Status500InternalServerError
        };

        context.Response.StatusCode = statusCode;

        var problem = new ExtendedProblemDetails
        {
            Status = statusCode,
            Title = ex switch
            {
                ValidationException => _localizer["ValidationFailed"],
                NotFoundException => _localizer["NotFound"],
                BusinessException => _localizer["BusinessError"],
                JsonException => _localizer["InvalidJson"],
                _ => _localizer[ex.GetType().Name]
            },
            Detail = _env.IsDevelopment() ? ex.ToString() : ex.Message,
            Instance = context.Request.Path,
            Type = $"https://httpstatuses.com/{statusCode}",
            ErrorCode = ex.GetType().Name,
            ErrorSource = ex.Source,
            Errors = ex.Data?.Count > 0 ? ex.Data : null
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(problem,
            new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase, WriteIndented = true }));
    }
}
#endregion
