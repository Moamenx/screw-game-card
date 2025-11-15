using Microsoft.AspNetCore.SignalR;
using ScrewGameCard.Application.Contract;
using ScrewGameCard.Application.Repository;
using ScrewGameCard.Infrastructure.SignalR;

namespace ScrewGameCard.Infrastructure.Services
{
    public class GameNotificationService : IGameNotificationService
    {
        private readonly IHubContext<GameHub> _hubContext;
        private readonly IGameRoomRepository _gameRoomRepository;

        public GameNotificationService(IHubContext<GameHub> hubContext, IGameRoomRepository gameRoomRepository)
        {
            _hubContext = hubContext;
            _gameRoomRepository = gameRoomRepository;
        }

        public Task NotifyRoomUpdateAsync(Guid roomId)
        {
            throw new NotImplementedException();
        }

        public Task NotifyGameStartedAsync(Guid roomId)
        {
            throw new NotImplementedException();
        }

        public Task NotifyPlayerJoinedAsync(Guid roomId, string playerName)
        {
            throw new NotImplementedException();
        }

        public Task NotifyPlayerLeftAsync(Guid roomId, string playerName)
        {
            throw new NotImplementedException();
        }

        public Task NotifyErrorAsync(string connectionId, string message)
        {
            throw new NotImplementedException();
        }
    }
}
