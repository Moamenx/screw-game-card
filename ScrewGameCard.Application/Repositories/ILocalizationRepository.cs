using ScrewGameCard.Application.Repositories;
using ScrewGameCard.Domain.Entities;

namespace ScrewGameCard.Application.Contract;

public interface ILocalizationRepository : IGenericRepository<Localization>
{
    Task<string?> GetMessageAsync(string code, string language);
}