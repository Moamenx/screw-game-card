using ScrewGameCard.Domain.Entities.Common;

namespace ScrewGameCard.Domain.Entities
{
    public class Leaderboard : IAuditable
    {
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
