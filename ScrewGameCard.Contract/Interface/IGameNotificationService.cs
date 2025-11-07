using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrewGameCard.Contract.Interface
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
