using AutoMapper;
using MarketPOS.Application.Common.Exceptions;
using MarketPOS.Shared.ExceptionDto;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using FluentValidation;

namespace MarketPOS.Infrastructure.Exceptions;
public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly IHostEnvironment _env;
    private readonly IStringLocalizer<ExceptionMiddleware> _localizer;

    public ExceptionMiddleware(
        RequestDelegate next,
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
            _logger.LogError(ex, _localizer["InternalError"]);
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        context.Response.ContentType = "application/json";

        // 1. Handle ResultDtoException (custom business logic)
        if (ex is ResultDtoException resultEx)
        {
            context.Response.StatusCode = resultEx.StatusCode;

            var result = new ExtendedProblemDetails
            {
                Title = _localizer["BusinessError"],
                Detail = _localizer[resultEx.Message],
                Status = resultEx.StatusCode,
                Instance = context.Request.Path,
                Type = $"https://httpstatuses.com/{resultEx.StatusCode}",
                ErrorSource = resultEx.Source
            };

            var json = JsonSerializer.Serialize(result, options: JsonOptions());
            await context.Response.WriteAsync(json);
            return;
        }

        // 2. Handle Validation Errors (FluentValidation)
        if (ex is ValidationException validationEx)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;

            var validationProblem = new ExtendedProblemDetails
            {
                Title = _localizer["ValidationFailed"],
                Detail = _localizer["Please review the input data."],
                Status = StatusCodes.Status400BadRequest,
                Instance = context.Request.Path,
                Type = "https://httpstatuses.com/400",
                ErrorCode = "ValidationError",
                ErrorSource = ex.Source
            };

            var json = JsonSerializer.Serialize(validationProblem, JsonOptions());
            await context.Response.WriteAsync(json);
            return;
        }

        // 3. Handle NotFound
        if (ex is NotFoundException notFoundEx)
        {
            context.Response.StatusCode = StatusCodes.Status404NotFound;

            var result = new ExtendedProblemDetails
            {
                Title = _localizer["NotFound"],
                Detail = _localizer[notFoundEx.Message],
                Status = StatusCodes.Status404NotFound,
                Instance = context.Request.Path,
                Type = "https://httpstatuses.com/404",
                ErrorCode = "NotFound",
                ErrorSource = notFoundEx.Source
            };

            var json = JsonSerializer.Serialize(result, JsonOptions());
            await context.Response.WriteAsync(json);
            return;
        }

        // 4. Default Exception Handler
        var statusCode = ex switch
        {
            UnauthorizedAccessException => StatusCodes.Status403Forbidden,
            ArgumentNullException => StatusCodes.Status400BadRequest,
            ArgumentException => StatusCodes.Status400BadRequest,
            DbUpdateException => StatusCodes.Status500InternalServerError,
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
            Instance = context.Request.Path,
            Type = $"https://httpstatuses.com/{statusCode}",
            Title = _localizer[ex.GetType().Name],
            Detail = _env.IsDevelopment() ? ex.ToString() : _localizer["Something went wrong."],
            ErrorSource = ex.Source
        };

        // If AutoMapper error, try extract more info
        if (ex is AutoMapperMappingException mapEx)
        {
            problem.Title = _localizer["MappingError"];
            problem.ErrorCode = mapEx.InnerException?.GetType().Name;
            problem.Detail = mapEx.InnerException?.Message ?? _localizer["Mapping failed."];
            problem.ErrorSource = mapEx.MemberMap?.DestinationName ?? ex.Source;
        }

        var jsonResponse = JsonSerializer.Serialize(problem, JsonOptions());
        await context.Response.WriteAsync(jsonResponse);
    }

    private static JsonSerializerOptions JsonOptions() =>
        new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };
}
