using ScrewGameCard.Domain.Entities;

namespace ScrewGameCard.Application.Contract;

public interface ILocalizationService
{
    Task<string> GetMessageAsync(string code, string language = "en");
    Task LoadAllAsync();
}