using ScrewGameCard.Application.Contract;
using ScrewGameCard.Application.DTO.CreateRoom;
using ScrewGameCard.Application.DTO.Game;
using ScrewGameCard.Application.DTO.JoinRoom;
using ScrewGameCard.Application.DTO.LeaveRoom;
using ScrewGameCard.Application.DTO.Player;
using ScrewGameCard.Application.Repositories;
using ScrewGameCard.Domain.Entities;
using ScrewGameCard.Domain.Enums;

namespace ScrewGameCard.Application.Service
{
    public class GameManager : IGameManager
    {
        private readonly IGameRepository _gameRepository;
        private readonly IGenericRepository<Player> _playerRepository;
        private readonly IGenericRepository<GamePlayer> _gamePlayerRepository;
        private readonly IGenericRepository<Round> _roundRepository;

        public GameManager(IGameRepository gameRepository, IGenericRepository<Player> playerRepository, IGenericRepository<GamePlayer> gamePlayerRepository, IGenericRepository<Round> roundRepository)
        {
            _gameRepository = gameRepository ?? throw new ArgumentNullException(nameof(gameRepository));
            _playerRepository = playerRepository ?? throw new ArgumentNullException(nameof(playerRepository));
            _gamePlayerRepository = gamePlayerRepository ?? throw new ArgumentNullException(nameof(gamePlayerRepository));
            _roundRepository = roundRepository ?? throw new ArgumentNullException(nameof(roundRepository));
        }

        public async Task<CreateRoomResponse> CreateGameAsync(CreateGameRequest request)
        {
            var host = await _playerRepository.GetAsync(request.Host.Id);
            if (host == null)
            {
                return new CreateRoomResponse { IsCreated = false };
            }

            var game = new Game
            {
                Name = request.RoomName,
                RoomPasscode = request.PassCode,
                NumberOfPlayers = request.MaximumNumberOfPlayers,
                HostId = host.Id,
                Status = GameStatus.Waiting,
                IsPrivate = !string.IsNullOrEmpty(request.PassCode),
                IsFull = false,
                DurationPerTurnInSeconds = 30,
                IsDoubleGameRandomized = true,
                CreatedDate = DateTime.Now
            };

            await _gameRepository.AddAsync(game);

            var gamePlayer = new GamePlayer
            {
                GameId = game.Id,
                PlayerId = host.Id,
                Position = 1,
                Status = GamePlayerStatus.Ready,
                JoinedAt = DateTime.Now,
                CreatedDate = DateTime.Now
            };
            game.Players.Add(gamePlayer);
            await _gamePlayerRepository.AddAsync(gamePlayer);

            var gameDto = new DTO.GameDto
            {
                Id = game.Id,
                Name = game.Name,
                Status = GameStatus.Waiting
            };

            return new CreateRoomResponse { Game = gameDto, IsCreated = true };
        }

        public async Task<JoinRoomResponse> JoinGameAsync(JoinGameRequest request)
        {
            var game = await _gameRepository.GetAsync(request.GameId);
            if (game == null || game.IsFull || game.Status != GameStatus.Waiting)
            {
                return new JoinRoomResponse { IsJoined = false };
            }

            var player = await _playerRepository.GetAsync(request.Player.Id);
            if (player == null)
            {
                return new JoinRoomResponse { IsJoined = false };
            }

            var existing = await _gamePlayerRepository.FirstOrDefaultAsync(gp => gp.GameId == game.Id && gp.PlayerId == player.Id);
            if (existing != null)
            {
                return new JoinRoomResponse { IsJoined = false };
            }

            var position = game.Players.Count + 1;
            var gamePlayer = new GamePlayer
            {
                GameId = game.Id,
                PlayerId = player.Id,
                Position = position,
                Status = GamePlayerStatus.Ready,
                JoinedAt = DateTime.Now,
                CreatedDate = DateTime.Now
            };

            await _gamePlayerRepository.AddAsync(gamePlayer);

            game.IsFull = position >= game.NumberOfPlayers;
            await _gameRepository.UpdateAsync(game);

            return new JoinRoomResponse { IsJoined = true };
        }

        public async Task<bool> StartGameAsync(Guid gameId)
        {
            var game = await _gameRepository.GetAsync(gameId);
            if (game == null || game.Status != GameStatus.Waiting || game.Players.Count < 2)
            {
                return false;
            }

            game.Status = GameStatus.Started;

            // Create 5 rounds
            for (int i = 1; i <= 5; i++)
            {
                var round = new Round
                {
                    GameId = game.Id,
                    RoundNumber = i,
                    IsDouble = false, // Will randomize later
                    StartedAt = DateTime.Now
                };
                await _roundRepository.AddAsync(round);
            }

            await _gameRepository.UpdateAsync(game);
            return true;
        }

        public async Task<GameStateResponse?> GetGameStateAsync(Guid gameId, string? forPlayerId = null)
        {
            var game = await _gameRepository.GetAsync(gameId);
            if (game == null)
            {
                return null;
            }

            var players = game.Players.Select(gp => new PlayerStateDto
            {
                Id = gp.Position,
                Name = gp.Player.Name,
                Score = gp.Score
            }).ToList();

            var currentRound = game.Rounds.Count(r => r.EndedAt.HasValue) + 1;
            var currentPlayerId = game.Players.OrderBy(gp => gp.Position).FirstOrDefault()?.PlayerId.ToString() ?? string.Empty;

            return new GameStateResponse
            {
                Players = players,
                CurrentRound = currentRound,
                CurrentPlayerId = currentPlayerId
            };
        }

        public async Task<bool> SetPlayerReadyAsync(string connectionId, bool ready)
        {
            // Assume connectionId is playerId as string
            if (!Guid.TryParse(connectionId, out var playerId))
            {
                return false;
            }

            var gamePlayer = await _gamePlayerRepository.FirstOrDefaultAsync(gp => gp.PlayerId == playerId && gp.LeftAt == null);
            if (gamePlayer == null)
            {
                return false;
            }

            // Assuming Status can be Ready or Joined
            gamePlayer.Status = ready ? GamePlayerStatus.Ready : GamePlayerStatus.Ready;
            await _gamePlayerRepository.UpdateAsync(gamePlayer);
            return true;
        }

        public async Task<LeaveRoomResponse> LeaveGameAsync(string connectionId)
        {
            if (!Guid.TryParse(connectionId, out var playerId))
            {
                return new LeaveRoomResponse { IsLeft = false };
            }

            var gamePlayer = await _gamePlayerRepository.FirstOrDefaultAsync(gp => gp.PlayerId == playerId && gp.LeftAt == null);
            if (gamePlayer == null)
            {
                return new LeaveRoomResponse { IsLeft = false };
            }

            gamePlayer.LeftAt = DateTime.Now;
            gamePlayer.Status = GamePlayerStatus.Left;
            await _gamePlayerRepository.UpdateAsync(gamePlayer);

            return new LeaveRoomResponse { IsLeft = true, PlayerName = gamePlayer.Player.Name };
        }

        public async Task<bool> IsPlayerInAnyRoom(string playerId)
        {
            if (!Guid.TryParse(playerId, out var id))
            {
                return false;
            }

            return await _gamePlayerRepository.ExistsAsync(gp => gp.PlayerId == id && gp.LeftAt == null);
        }
    }
}
