using ScrewGameCard.Application.Contract;
using ScrewGameCard.Application.DTO;
using ScrewGameCard.Application.DTO.Common;
using ScrewGameCard.Application.Mapping;
using ScrewGameCard.Application.Repositories;
using ScrewGameCard.Domain.Enums;

namespace ScrewGameCard.Application.Service
{
    public class GameService : IGameService
    {
        private readonly IGameRepository _gameRepository;
        public GameService(IGameRepository gameRepository)
        {
            _gameRepository = gameRepository ?? throw new ArgumentNullException(nameof(gameRepository));
        }
        public async Task<PaginatedResult<GameDto>> GetGamesAsync(int pageNumber = 1, int pageSize = 10)
        {
            var activeGames = await _gameRepository.GetPaginatedAsync(pageNumber, pageSize, g => g.Status != GameStatus.Ended);
            var games = activeGames.Data.Select(game => game.ToGameDto()).ToList();
            return PaginatedResult<GameDto>.Create(games, pageNumber, pageSize, activeGames.TotalRecords);
        }
    }
}
