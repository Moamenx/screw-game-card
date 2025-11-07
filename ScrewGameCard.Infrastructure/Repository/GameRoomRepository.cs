using ScrewGameCard.Contract.Repository;
using ScrewGameCard.Domain.Entities;
using System.Collections.Concurrent;

namespace ScrewGameCard.Infrastructure.Repository
{
    public class GameRoomRepository : IGameRoomRepository
    {
        private readonly ConcurrentDictionary<Guid, GameRoom> _rooms = new();
        private readonly ConcurrentDictionary<string, Guid> _playerToRoom = new();

        public Task<GameRoom?> GetByIdAsync(Guid roomId)
        {
            _rooms.TryGetValue(roomId, out var room);
            return Task.FromResult(room);
        }

        public Task<GameRoom?> GetByConnectionIdAsync(string connectionId)
        {
            return _playerToRoom.TryGetValue(connectionId, out var roomId) ? GetByIdAsync(roomId) : Task.FromResult<GameRoom?>(null);
        }

        public Task<List<GameRoom>> GetAvailableRoomsAsync()
        {
            var availableRooms = _rooms.Values
                .Where(r => r.State == Domain.Enums.GameState.Waiting && !r.IsFull)
                .ToList();
            return Task.FromResult(availableRooms);
        }

        public Task<GameRoom> AddAsync(GameRoom room)
        {
            _rooms[room.Id] = room;
            return Task.FromResult(room);
        }

        public Task UpdateAsync(GameRoom room)
        {
            _rooms[room.Id] = room;
            return Task.CompletedTask;
        }

        public Task RemoveAsync(Guid roomId)
        {
            _rooms.TryRemove(roomId, out _);

            // Clean up player mappings
            var playersToRemove = _playerToRoom
                .Where(kvp => kvp.Value == roomId)
                .Select(kvp => kvp.Key)
                .ToList();

            foreach (var playerId in playersToRemove)
            {
                _playerToRoom.TryRemove(playerId, out _);
            }

            return Task.CompletedTask;
        }

        public Task MapPlayerToRoomAsync(string connectionId, Guid roomId)
        {
            _playerToRoom[connectionId] = roomId;
            return Task.CompletedTask;
        }

        public Task UnmapPlayerFromRoomAsync(string connectionId)
        {
            _playerToRoom.TryRemove(connectionId, out _);
            return Task.CompletedTask;
        }
    }
}
