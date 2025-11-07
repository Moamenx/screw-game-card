using ScrewGameCard.Contract.DTO.CreateRoom;
using ScrewGameCard.Contract.DTO.Game;
using ScrewGameCard.Contract.DTO.JoinRoom;
using ScrewGameCard.Contract.DTO.LeaveRoom;
using ScrewGameCard.Contract.DTO.Player;
using ScrewGameCard.Contract.Interface;
using ScrewGameCard.Contract.Repository;

namespace ScrewGameCard.Application.Service
{
    public class GameRoomService : IGameRoomService
    {
        private readonly IGameRoomRepository _gameRoomRepository;
        private readonly IGameEngine _gameEngine;

        public GameRoomService(IGameRoomRepository gameRoomRepository, IGameEngine gameEngine)
        {
            _gameRoomRepository = gameRoomRepository;
            _gameEngine = gameEngine;
        }

        public async Task<CreateRoomResponse> CreateRoomAsync(CreateRoomRequest request)
        {
            var result = new CreateRoomResponse();
            var room = new GameRoom(request.RoomName);
            var player = new Player(request.Player.Name, request.Player.ConnectionId);
            room.AddPlayer(player);
            room = await _gameRoomRepository.AddAsync(room);
            await _gameRoomRepository.MapPlayerToRoomAsync(request.Player.ConnectionId, room.Id);

            return result;
        }

        public async Task<JoinRoomResponse> JoinRoomAsync(JoinRoomRequest request)
        {
            var result = new JoinRoomResponse();
            var room = await _gameRoomRepository.GetByIdAsync(request.RoomId);
            if (room == null)
                throw new Exception("Room not found");

            var player = new Player(request.Player.Name, request.Player.ConnectionId);
            room.AddPlayer(player);

            await _gameRoomRepository.UpdateAsync(room);
            await _gameRoomRepository.MapPlayerToRoomAsync(request.Player.ConnectionId, room.Id);
            result.IsJoined = true;
            return result;
        }

        public Task<bool> StartGameAsync(Guid roomId)
        {
            throw new NotImplementedException();
        }

        public Task<GameStateResponse?> GetGameStateAsync(Guid roomId, string? forPlayerId = null)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SetPlayerReadyAsync(string connectionId, bool ready)
        {
            throw new NotImplementedException();
        }

        public async Task<LeaveRoomResponse> LeaveRoomAsync(string connectionId)
        {
            var result = new LeaveRoomResponse();
            var room = await _gameRoomRepository.GetByConnectionIdAsync(connectionId);
            if (room == null)
                throw new Exception("Player is not in the room");


            var player = room.Players.FirstOrDefault(p => p.ConnectionId == connectionId);
            if (player == null) 
                return result;

            room.RemovePlayer(player);
            await _gameRoomRepository.UpdateAsync(room);
            await _gameRoomRepository.UnmapPlayerFromRoomAsync(connectionId);

            result.IsLeft = true;
            result.PlayerName = player.Name;

            if (room.Players.Count == 0)
                await _gameRoomRepository.RemoveAsync(room.Id);
            
            return result;

        }
    }
}
