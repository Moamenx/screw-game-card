namespace ScrewGameCard.Application.Contract
{
    public interface IGameNotificationService
    {
        Task NotifyRoomUpdateAsync(Guid roomId);
        Task NotifyGameStartedAsync(Guid roomId);
        Task NotifyPlayerJoinedAsync(Guid roomId, string playerName);
        Task NotifyPlayerLeftAsync(Guid roomId, string playerName);
        Task NotifyErrorAsync(string connectionId, string message);
    }
}
