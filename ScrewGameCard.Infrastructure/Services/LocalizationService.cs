using ScrewGameCard.Application.Contract;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using ScrewGameCard.DomainShared;

namespace ScrewGameCard.Infrastructure.Services;

public class LocalizationService : ILocalizationService
{
    private readonly ILocalizationRepository _repository;
    private readonly IMemoryCache _cache;
    private readonly CachingDurationOption _cacheOptions;

    public LocalizationService(ILocalizationRepository repository, IMemoryCache cache, IOptions<CachingDurationOption> cacheOptions)
    {
        _repository = repository;
        _cache = cache;
        _cacheOptions = cacheOptions.Value;
    }

    public async Task<string> GetMessageAsync(string code, string language = "en")
    {
        var cacheKey = $"{code}_{language}";

        if (!_cache.TryGetValue(cacheKey, out string? message))
        {
            message = await _repository.GetMessageAsync(code, language) ?? code;
            _cache.Set(cacheKey, message, TimeSpan.FromHours(_cacheOptions.LocalizationCacheDurationInHours));
        }

        return message;
    }

    public async Task LoadAllAsync()
    {
        var allLocalizations = await _repository.GetAllAsync();
        foreach (var loc in allLocalizations)
        {
            var cacheKey = $"{loc.Code}_{loc.Language}";
            _cache.Set(cacheKey, loc.Message, TimeSpan.FromHours(_cacheOptions.LocalizationCacheDurationInHours));
        }
    }
}