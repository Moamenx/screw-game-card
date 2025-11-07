using ScrewGameCard.Domain.Entities.Common;
using ScrewGameCard.Domain.Enums;

namespace ScrewGameCard.Domain.Entities
{
    public class Player : IAuditable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string AvatarUrl { get; set; }
        public double WinLoseRatio { get; set; }
        public int NumberOfGamesPlayed { get; set; }
        public int NumberOfWonGames { get; set; }
        public int NumberOfLostGames { get; set; }
        public int Coins { get; set; }
        public int Rank { get; set; }
        public PlayerStatus Status { get; set; }
        public string Language { get; set; }
        public DateTime LastLoggedInTime { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual ICollection<Friendship> Friendships { get; set; }
        public virtual ICollection<Friendship> FriendOf { get; set; }

        public Player()
        {
            Friendships = new List<Friendship>();
            FriendOf = new List<Friendship>();
        }
    }
}
