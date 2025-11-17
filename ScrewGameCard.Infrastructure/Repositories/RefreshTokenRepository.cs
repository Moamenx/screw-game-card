using ScrewGameCard.Application.Repositories;
using ScrewGameCard.Domain.Entities;
using ScrewGameCard.Infrastructure.Data;

namespace ScrewGameCard.Infrastructure.Repositories;

public class RefreshTokenRepository : GenericRepository<RefreshToken>, IGenericRepository<RefreshToken>
{
    public RefreshTokenRepository(ScrewGameCardDbContext context) : base(context)
    {
    }
}