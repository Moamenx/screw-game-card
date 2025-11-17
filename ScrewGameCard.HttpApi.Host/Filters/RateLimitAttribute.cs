using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ScrewGameCard.Application.Contract;

namespace ScrewGameCard.HttpApi.Host.Filters;

public class RateLimitAttribute : ActionFilterAttribute
{
    private readonly string _policyName;
    private readonly int? _limit;
    private readonly int? _windowMinutes;

    public RateLimitAttribute(string policyName)
    {
        _policyName = policyName ?? throw new ArgumentNullException(nameof(policyName));
    }
    public RateLimitAttribute(int limit, int windowMinutes)
    {
        _limit = limit;
        _windowMinutes = windowMinutes;
    }
    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var rateLimiter = context.HttpContext.RequestServices.GetRequiredService<IRateLimiter>();
        var ip = context.HttpContext.Connection.RemoteIpAddress?.MapToIPv4()?.ToString() ?? "unknown";

        var result = await rateLimiter.IsRequestAllowedAsync(ip, _policyName);

        if (!result.Allowed)
        {
            context.Result = new StatusCodeResult(429);
            AddRateLimitHeaders(context.HttpContext.Response, result);
            var problemDetails = new ProblemDetails
            {
                Title = "Too Many Requests",
                Detail = $"Rate limit exceeded. Try again in {result.RetryAfterSeconds?.TotalSeconds ?? 60} seconds.",
                Status = StatusCodes.Status429TooManyRequests,
                Instance = context.HttpContext.Request.Path,
                Extensions =
            {
                ["limit"] = result.Limit,
                ["remaining"] = result.RemainingCount,
                ["retryAfter"] = result.RetryAfterSeconds?.TotalSeconds,
                ["window"] = result.Window.ToString()
            }
            };

            context.Result = new ObjectResult(problemDetails) { StatusCode = StatusCodes.Status429TooManyRequests };
            return;
        }

        await next();
    }
    private void AddRateLimitHeaders(HttpResponse response, RateLimitResult result)
    {
        response.Headers["X-RateLimit-Limit"] = result.Limit.ToString();
        response.Headers["X-RateLimit-Remaining"] = result.RemainingCount.ToString();
        response.Headers["X-RateLimit-Reset"] = result.RetryAfter?.ToString("R") ?? DateTime.UtcNow.Add(result.Window).ToString("R");
    }
}