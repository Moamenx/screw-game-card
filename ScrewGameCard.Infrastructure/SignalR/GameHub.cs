using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using ScrewGameCard.Application.Contract;
using ScrewGameCard.Application.DTO.CreateRoom;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using ScrewGameCard.Application.DTO;

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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<GameDto> CreateGame(CreateGameRequest request)
        {
            if (await _gameManager.IsPlayerInAnyRoom(Context.UserIdentifier))
                throw new HubException("You are already in a room");

            request?.Host.Id = Guid.Parse(Context.UserIdentifier);
            var creationResult =  await _gameManager.CreateGameAsync(request);

            if(!creationResult.IsCreated)
                throw new HubException("Something went wrong. Please try again");

            return creationResult.Game;

        }
    }
}
