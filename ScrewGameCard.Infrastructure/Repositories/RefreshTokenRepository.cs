using ScrewGameCard.Application.Repository;
using ScrewGameCard.Domain.Entities;
using ScrewGameCard.Infrastructure.Data;
using ScrewGameCard.Infrastructure.Repository;

namespace ScrewGameCard.Infrastructure.Repositories;

public class RefreshTokenRepository : GenericRepository<RefreshToken>, IGenericRepository<RefreshToken>
{
    public RefreshTokenRepository(ScrewGameCardDbContext context) : base(context)
    {
    }
}