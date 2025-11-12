using ScrewGameCard.Application.DTO.Player;
using ScrewGameCard.Domain.Enums;

namespace ScrewGameCard.Application.DTO.CreateRoom
{
    public record CreateGameRequest
    {
        public required string RoomName { get; set; }
        public string? PassCode { get; set; }
        public int MaximumNumberOfPlayers { get; set; }
        public GameType Type { get; set; }
        public required PlayerDto Host { get; set; }
    }
}
