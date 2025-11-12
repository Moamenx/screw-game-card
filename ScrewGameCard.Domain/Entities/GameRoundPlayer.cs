namespace ScrewGameCard.Domain.Entities
{
    public class GameRoundPlayer
    {
        public Guid Id { get; set; }
        public Guid GameId { get; set; }
        public Guid GamePlayerId { get; set; }
        public Guid RoundId { get; set; }
        public int Score { get; set; }
        public virtual Game Game { get; set; }
        public virtual GamePlayer GamePlayer { get; set; }
        public virtual Round Round { get; set; }
    }
}
