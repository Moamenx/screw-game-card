namespace ScrewGameCard.Application.Contract;

public interface IRateLimiter
{
    Task<RateLimitResult> IsRequestAllowedAsync(string key, int limit, TimeSpan window);
    Task<RateLimitResult> IsRequestAllowedAsync(string key, string policyName);
}

public class RateLimitResult
{
    public bool Allowed { get; set; }
    public int RemainingCount { get; set; }
    public int Limit { get; set; }
    public TimeSpan Window { get; set; }
    public DateTime? RetryAfter { get; set; }
    public TimeSpan? RetryAfterSeconds => RetryAfter.HasValue ? RetryAfter.Value - DateTime.Now : null;
}