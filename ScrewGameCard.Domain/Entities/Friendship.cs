using ScrewGameCard.Domain.Entities.Common;
using ScrewGameCard.Domain.Enums;

namespace ScrewGameCard.Domain.Entities
{
    public class Friendship : IAuditable
    {
        public int Id { get; set; }
        public int PlayerId { get; set; }
        public int FriendId { get; set; }
        public FriendshipStatus Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public virtual Player Player { get; set; }
        public virtual Player Friend { get; set; }
    }
}
