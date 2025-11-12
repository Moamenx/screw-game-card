namespace ScrewGameCard.Domain.Entities
{
    public class Round
    {
        public Guid Id { get; set; }
        public Guid GameId { get; set; }
        public int RoundNumber { get; set; }
        public bool IsDouble { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime? EndedAt { get; set; }

        public virtual Game Game{ get; set; }

        public virtual ICollection<GameRoundPlayer> Players { get; set; } = new List<GameRoundPlayer>();

    }
}
