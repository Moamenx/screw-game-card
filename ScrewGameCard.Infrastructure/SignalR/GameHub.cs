using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScrewGameCard.Contract.Repository;

namespace ScrewGameCard.Infrastructure.SignalR
{
    public class GameHub : Hub
    {
        private readonly IGameRoomRepository _gameRoomRepository;   
        public GameHub(IGameRoomRepository gameRoomRepository)
        {
            _gameRoomRepository = gameRoomRepository;
        }
        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }

        
    }
}
