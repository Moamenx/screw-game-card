using ScrewGameCard.Domain.Entities;

namespace ScrewGameCard.Contract.Repository
{
    public interface IGameRoomRepository
    {
        Task<GameRoom?> GetByIdAsync(Guid roomId);
        Task<GameRoom?> GetByConnectionIdAsync(string connectionId);
        Task<List<GameRoom>> GetAvailableRoomsAsync();
        Task<GameRoom?> AddAsync(GameRoom room);
        Task UpdateAsync(GameRoom room);
        Task RemoveAsync(Guid roomId);
        Task MapPlayerToRoomAsync(string connectionId, Guid roomId);
        Task UnmapPlayerFromRoomAsync(string connectionId);
    }
}
