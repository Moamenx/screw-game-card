using Microsoft.Extensions.Configuration;
using ScrewGameCard.Application.Contract;
using System.Collections.Concurrent;

namespace ScrewGameCard.Infrastructure.Services;

public class RateLimiterService : IRateLimiter
{
    private static ConcurrentDictionary<string, LinkedList<DateTime>> _requestTimes = new();
    private readonly IConfiguration _config;

    public RateLimiterService(IConfiguration config)
    {
        _config = config ?? throw new ArgumentNullException(nameof(config));
    }

    public Task<RateLimitResult> IsRequestAllowedAsync(string key, int limit, TimeSpan window)
    {
        var now = DateTime.Now;
        var cutoff = now - window;

        var requests = _requestTimes.GetOrAdd(key, _ => new LinkedList<DateTime>());

        lock (requests)
        {
            // Remove expired requests from the front
            while (requests.First != null && requests.First.Value < cutoff)
            {
                requests.RemoveFirst();
            }

            if (requests.Count < limit)
            {
                requests.AddLast(now);
                return Task.FromResult(new RateLimitResult
                {
                    Allowed = true,
                    RemainingCount = limit - requests.Count,
                    Limit = limit,
                    Window = window
                });
            }
            else
            {
                var retryAfter = requests.First?.Value.Add(window) ?? now.Add(window);
                return Task.FromResult(new RateLimitResult
                {
                    Allowed = false,
                    RemainingCount = 0,
                    Limit = limit,
                    Window = window,
                    RetryAfter = retryAfter
                });
            }
        }
    }

    public Task<RateLimitResult> IsRequestAllowedAsync(string key, string policyName)
    {
        var limit = _config.GetValue<int>($"RateLimits:{policyName}:Limit");
        var windowMinutes = _config.GetValue<int>($"RateLimits:{policyName}:WindowMinutes");

        if (limit == 0) limit = 10; // default
        if (windowMinutes == 0) windowMinutes = 1;

        return IsRequestAllowedAsync(key, limit, TimeSpan.FromMinutes(windowMinutes));
    }
}