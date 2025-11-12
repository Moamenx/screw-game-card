using Microsoft.AspNetCore.SignalR;
using ScrewGameCard.Application.Contract;
using ScrewGameCard.Application.DTO.CreateRoom;

namespace ScrewGameCard.Infrastructure.SignalR
{
    public class GameHub : Hub<IGameClient>
    {
        private readonly IGameManager _gameManager;

        public GameHub(IGameManager gameManager)
        {
            _gameManager = gameManager ?? throw new ArgumentNullException(nameof(gameManager));
        }

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }

        public async Task CreateGame(CreateGameRequest request)
        {
            var playerId = Context.ConnectionId;

            if (await _gameManager.IsPlayerInAnyRoom(playerId))
                await Clients.Caller.Error("You are already in a room");

            var creationResult =  await _gameManager.CreateGameAsync(request);

            if(!creationResult.IsCreated)
                await Clients.Caller.Error("Something went wrong. Please try again");

            await Clients.Caller.GameCreated(creationResult?.Game);

        }
    }
}
