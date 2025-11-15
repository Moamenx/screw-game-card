using ScrewGameCard.Domain.Entities.Common;
using ScrewGameCard.Domain.Enums;

namespace ScrewGameCard.Domain.Entities
{
    public class Game :IAuditable
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? RoomPasscode { get; set; }
        public bool IsPrivate { get; set; }
        public int DurationPerTurnInSeconds { get; set; }
        public int NumberOfPlayers { get; set; }
        public bool IsDoubleGameRandomized { get; set; }
        public virtual Player Host { get; set; }
        public bool IsFull { get; set; }
        public GameStatus Status { get; set; }
        public Guid HostId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public ICollection<GamePlayer> Players { get; set; } = new List<GamePlayer>();
        public ICollection<Round> Rounds { get; set; } = new List<Round>();

    }
}
