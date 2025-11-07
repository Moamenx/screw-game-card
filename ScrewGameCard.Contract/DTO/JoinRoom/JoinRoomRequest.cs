using ScrewGameCard.Contract.DTO.Player;

namespace ScrewGameCard.Contract.DTO.JoinRoom
{
    public class JoinRoomRequest
    {
        public Guid RoomId { get; set; }
        public PlayerDto Player { get; set; }
    }
}
