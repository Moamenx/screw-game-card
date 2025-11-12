using ScrewGameCard.Domain.Entities.Common;
using ScrewGameCard.Domain.Enums;

namespace ScrewGameCard.Domain.Entities
{
    public class Player : IAuditable
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string AvatarUrl { get; set; }
        public double WinLoseRatio { get; set; }
        public int NumberOfGamesPlayed { get; set; }
        public int NumberOfWonGames { get; set; }
        public int NumberOfLostGames { get; set; }
        public int Coins { get; set; }
        public int Rank { get; set; }
        public int Level { get; set; }
        public double Experience  { get; set; }
        public PlayerStatus Status { get; set; }
        public string Language { get; set; }
        public DateTime LastLoggedInTime { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
            
        public virtual ICollection<Friendship> Friendships { get; set; } = new List<Friendship>();
        public virtual ICollection<Friendship> FriendOf { get; set; } = new List<Friendship>();
        public virtual ICollection<GamePlayer> GamePlayers { get; set; } = new List<GamePlayer>();
        public virtual ICollection<Game> HostedGames { get; set; } = new List<Game>();

    }
}
