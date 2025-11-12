using ScrewGameCard.Domain.Enums;

namespace ScrewGameCard.Application.DTO.Game
{
    public class GameDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int MaximumNumberOfPlayers { get; set; }
        public int NumberOfPlayersInGame { get; set; }
        public GameStatus Status { get; set; }
        public GameType Type { get; set; }
        public bool RequiresPassword { get; set; }
        public bool IsFull { get; set; }

    }
}
