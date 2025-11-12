using ScrewGameCard.Application.Contract;
using ScrewGameCard.Application.DTO.CreateRoom;
using ScrewGameCard.Application.DTO.Game;
using ScrewGameCard.Application.DTO.JoinRoom;
using ScrewGameCard.Application.DTO.LeaveRoom;
using ScrewGameCard.Application.Repository;

namespace ScrewGameCard.Application.Service
{
    public class GameManager : IGameManager
    {
        private readonly IGameRepository _gameRepository;

        public GameManager(IGameRepository gameRepository)
        {
            _gameRepository = gameRepository ?? throw new ArgumentNullException(nameof(gameRepository));
        }

        public Task<CreateRoomResponse> CreateGameAsync(CreateGameRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<JoinRoomResponse> JoinGameAsync(JoinGameRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<bool> StartGameAsync(Guid gameId)
        {
            throw new NotImplementedException();
        }

        public Task<GameStateResponse?> GetGameStateAsync(Guid gameId, string? forPlayerId = null)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SetPlayerReadyAsync(string connectionId, bool ready)
        {
            throw new NotImplementedException();
        }

        public Task<LeaveRoomResponse> LeaveGameAsync(string connectionId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsPlayerInAnyRoom(string playerId)
        {
            throw new NotImplementedException();
        }
    }
}
