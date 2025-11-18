using ScrewGameCard.Domain.Entities;
using ScrewGameCard.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using ScrewGameCard.Application.Contract;

namespace ScrewGameCard.Infrastructure.Repositories;

public class LocalizationRepository : GenericRepository<Localization>, ILocalizationRepository
{
    public LocalizationRepository(ScrewGameCardDbContext context) : base(context) { }

    public async Task<string?> GetMessageAsync(string code, string language)
    {
        return await _context.Localizations
            .Where(l => l.Code == code && l.Language == language)
            .Select(l => l.Message)
            .FirstOrDefaultAsync();
    }
}