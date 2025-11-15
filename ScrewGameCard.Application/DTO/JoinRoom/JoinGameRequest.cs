using ScrewGameCard.Application.DTO.Player;

namespace ScrewGameCard.Application.DTO.JoinRoom
{
    public class JoinGameRequest
    {
        public Guid GameId { get; set; }
        public PlayerDto Player { get; set; }
    }
}
