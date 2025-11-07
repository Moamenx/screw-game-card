using Microsoft.AspNetCore.SignalR;
using ScrewGameCard.Contract.Interface;
using ScrewGameCard.Contract.Repository;

namespace ScrewGameCard.Infrastructure.SignalR
{
    public class GameHub : Hub<IGameClient>
    {
        private readonly IGameRoomRepository _gameRoomRepository;   
        public GameHub(IGameRoomRepository gameRoomRepository)
        {
            _gameRoomRepository = gameRoomRepository;
        }
        public override async Task OnConnectedAsync()
        {
            var co = Context.ConnectionId;
            await base.OnConnectedAsync();
        }

        
    }
}
