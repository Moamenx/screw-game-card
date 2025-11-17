using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ScrewGameCard.DomainShared;
using ScrewGameCard.DomainShared.Exceptions;

namespace ScrewGameCard.HttpApi.Host.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        var statusCode = ex switch
        {
            BadRequestException => StatusCodes.Status400BadRequest,
            UnauthorizedException => StatusCodes.Status401Unauthorized,
            ForbiddenException => StatusCodes.Status403Forbidden,
            NotFoundException => StatusCodes.Status404NotFound,
            _ => StatusCodes.Status500InternalServerError
        };

        _logger.LogError(ex, "An error occurred: {Message}", ex.Message);

        var correlationId = context.Request.Headers["X-Request-Id"].FirstOrDefault() ?? Guid.NewGuid().ToString();
        var errorCode = ex is AppException appEx ? appEx.ErrorCode : statusCode;

        // Expose message only for known exceptions
        string message = ex is AppException ? ex.Message : "An internal server error occurred.";

        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        var response = ApiResponse<object>.Error(message, correlationId, errorCode);
        await context.Response.WriteAsJsonAsync(response);
    }
}