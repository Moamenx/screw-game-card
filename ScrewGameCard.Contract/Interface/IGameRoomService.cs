using ScrewGameCard.Contract.DTO.CreateRoom;
using ScrewGameCard.Contract.DTO.Game;
using ScrewGameCard.Contract.DTO.JoinRoom;
using ScrewGameCard.Contract.DTO.LeaveRoom;

namespace ScrewGameCard.Contract.Interface
{
    public interface IGameRoomService
    {
        Task<CreateRoomResponse> CreateRoomAsync(CreateRoomRequest request);

        Task<JoinRoomResponse> JoinRoomAsync(JoinRoomRequest request);

        Task<bool> StartGameAsync(Guid roomId);

        //Task<ActionResult> ExecuteActionAsync(GameActionRequest request);

        Task<GameStateResponse?> GetGameStateAsync(Guid roomId, string? forPlayerId = null);

        Task<bool> SetPlayerReadyAsync(string connectionId, bool ready);

        Task<LeaveRoomResponse> LeaveRoomAsync(string connectionId);
    }
}
