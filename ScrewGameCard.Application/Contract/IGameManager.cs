using ScrewGameCard.Application.DTO.CreateRoom;
using ScrewGameCard.Application.DTO.Game;
using ScrewGameCard.Application.DTO.JoinRoom;
using ScrewGameCard.Application.DTO.LeaveRoom;

namespace ScrewGameCard.Application.Contract
{
    public interface IGameManager
    {
        Task<CreateRoomResponse> CreateGameAsync(CreateGameRequest request);

        Task<JoinRoomResponse> JoinGameAsync(JoinGameRequest request);

        Task<bool> StartGameAsync(Guid gameId);

        //Task<ActionResult> ExecuteActionAsync(GameActionRequest request);

        Task<GameStateResponse?> GetGameStateAsync(Guid gameId, string? forPlayerId = null);

        Task<bool> SetPlayerReadyAsync(string connectionId, bool ready);

        Task<LeaveRoomResponse> LeaveGameAsync(string connectionId);

        Task<bool> IsPlayerInAnyRoom(string playerId);
    }
}
