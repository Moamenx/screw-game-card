using ScrewGameCard.Application.DTO.Player;

namespace ScrewGameCard.Application.DTO.JoinRoom
{
    public class JoinGameRequest
    {
        public Guid RoomId { get; set; }
        public PlayerDto Player { get; set; }
    }
}
