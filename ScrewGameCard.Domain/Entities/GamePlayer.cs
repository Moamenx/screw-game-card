using ScrewGameCard.Domain.Entities.Common;
using ScrewGameCard.Domain.Enums;

namespace ScrewGameCard.Domain.Entities
{
    public class GamePlayer : IAuditable
    {
        public Guid Id { get; set; }
        public Guid GameId { get; set; }
        public Guid PlayerId { get; set; }
        public int Position { get; set; }
        public GamePlayerStatus Status { get; set; } 
        public DateTime JoinedAt { get; set; }
        public DateTime? LeftAt { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int Score { get; set; }
        // Navigation properties
        public virtual Game Game { get; set; }
        public virtual Player Player { get; set; }
        public ICollection<Round> Rounds { get; set; } = new List<Round>();

    }
}
