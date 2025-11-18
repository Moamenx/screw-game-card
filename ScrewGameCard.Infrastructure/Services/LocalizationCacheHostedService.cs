using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ScrewGameCard.Application.Contract;

namespace ScrewGameCard.Infrastructure.Services;

public class LocalizationCacheHostedService : IHostedService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<LocalizationCacheHostedService> _logger;

    public LocalizationCacheHostedService(IServiceScopeFactory scopeFactory, ILogger<LocalizationCacheHostedService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Loading localization cache...");
        using (var scope = _scopeFactory.CreateScope())
        {
            var localizationService = scope.ServiceProvider.GetRequiredService<ILocalizationService>();
            await localizationService.LoadAllAsync();
        }
        _logger.LogInformation("Localization cache loaded successfully.");
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Localization cache service stopping.");
        return Task.CompletedTask;
    }
}