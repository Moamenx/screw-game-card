using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScrewGameCard.Application.DTO;
using ScrewGameCard.Application.DTO.Common;

namespace ScrewGameCard.Application.Contract
{
    public interface IGameService
    {
        Task<PaginatedResult<GameDto>> GetGamesAsync(int pageNumber = 1, int pageSize = 10);
    }
}
