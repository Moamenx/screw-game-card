using ScrewGameCard.Contract.DTO.Player;

namespace ScrewGameCard.Contract.DTO.CreateRoom
{
    public record CreateRoomRequest
    {
        public string RoomName { get; set; }
        public PlayerDto Player { get; set; }
    }
}
