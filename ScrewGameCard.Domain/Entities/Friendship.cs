using ScrewGameCard.Domain.Entities.Common;
using ScrewGameCard.Domain.Enums;

namespace ScrewGameCard.Domain.Entities
{
    public class Friendship : IAuditable
    {
        public Guid Id { get; set; }
        public Guid PlayerId { get; set; }
        public Guid FriendId { get; set; }
        public FriendshipStatus Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public virtual Player Player { get; set; }
        public virtual Player Friend { get; set; }
    }
}
