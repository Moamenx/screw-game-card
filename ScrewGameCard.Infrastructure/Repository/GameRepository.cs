using Microsoft.EntityFrameworkCore;
using ScrewGameCard.Application.Repository;
using ScrewGameCard.Domain.Entities;
using ScrewGameCard.Infrastructure.Data;

namespace ScrewGameCard.Infrastructure.Repository
{
    public class GameRepository : GenericRepository<Game>,IGameRepository
    {
        public GameRepository(ScrewGameCardDbContext context) : base(context)
        {
        }
    }
}
