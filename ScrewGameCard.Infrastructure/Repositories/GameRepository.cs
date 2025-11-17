using Microsoft.EntityFrameworkCore;
using ScrewGameCard.Application.Repositories;
using ScrewGameCard.Domain.Entities;
using ScrewGameCard.Infrastructure.Data;

namespace ScrewGameCard.Infrastructure.Repositories
{
    public class GameRepository : GenericRepository<Game>,IGameRepository
    {
        public GameRepository(ScrewGameCardDbContext context) : base(context)
        {
        }
    }
}
