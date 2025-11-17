using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ScrewGameCard.Infrastructure.Data;

namespace ScrewGameCard.Infrastructure.Services;

public class RefreshTokenCleanupService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<RefreshTokenCleanupService> _logger;

    public RefreshTokenCleanupService(IServiceScopeFactory scopeFactory, ILogger<RefreshTokenCleanupService> logger)
    {
        _scopeFactory = scopeFactory ?? throw new ArgumentNullException(nameof(scopeFactory));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("RefreshTokenCleanupService started");
        while (!stoppingToken.IsCancellationRequested)
        {
            await CleanupTokens(stoppingToken);
            await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
        }
        _logger.LogInformation("RefreshTokenCleanupService stopped");
    }

    private async Task CleanupTokens(CancellationToken stoppingToken)
    {
        try
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ScrewGameCardDbContext>();
                var deletedCount = await context.RefreshTokens
                    .Where(rt => rt.ExpiresAt < DateTime.Now || rt.IsRevoked)
                    .ExecuteDeleteAsync(stoppingToken);
                _logger.LogInformation("Cleaned up {DeletedCount} expired or revoked refresh tokens", deletedCount);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while cleaning up refresh tokens");
        }
    }
}