using Microsoft.AspNetCore.Mvc;
using ScrewGameCard.Application.Contract;
using ScrewGameCard.Application.DTO;
using ScrewGameCard.Application.DTO.Common;

namespace ScrewGameCard.HttpApi.Host.Controllers
{
    [ApiController]
    [Route("games")]

    public class GamesController : ControllerBase
    {
        private readonly IGameService _gameService;
        public GamesController(IGameService gameService)
        {
            _gameService = gameService ?? throw new ArgumentNullException(nameof(gameService));
        }

        [HttpGet]
        public async Task<PaginatedResult<GameDto>> GetGamesAsync(int pageNumber = 1, int pageSize = 10)
        {
           return await _gameService.GetGamesAsync(pageNumber, pageSize);
        }
    }
}
