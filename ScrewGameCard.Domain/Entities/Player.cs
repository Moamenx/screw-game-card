namespace ScrewGameCard.Domain.Entities
{
    public class Player
    {
        public string Id { get; private set; }
        public string ConnectionId { get; private set; }
        public string Name { get; private set; }
        public List<Card> Hand { get; private set; }
        public int Score { get; private set; }
        public int RoundScore { get; private set; }
        public bool IsReady { get; private set; }
        public bool HasUsedAction { get; private set; }

        public Player(string name, string connectionId)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new Exception("Player name cannot be empty");

            if (string.IsNullOrWhiteSpace(connectionId))
                throw new Exception("Connection ID cannot be empty");

            Id = Guid.NewGuid().ToString();
            Name = name;
            ConnectionId = connectionId;
            Hand = [];
            Score = 0;
            RoundScore = 0;
            IsReady = false;
            HasUsedAction = false;
        }

        public void SetReady(bool ready)
        {
            IsReady = ready;
        }

        public void AddCard(Card card)
        {
            if (card == null)
                throw new Exception("Cannot add null card");

            Hand.Add(card);
        }

        public void RemoveCard(Card card)
        {
            Hand.Remove(card);
        }

        public void ClearHand()
        {
            Hand.Clear();
        }

        public void MarkActionUsed()
        {
            HasUsedAction = true;
        }

        public void ResetActionUsed()
        {
            HasUsedAction = false;
        }

        public int CalculateRoundScore()
        {
            RoundScore = Hand.Sum(c => c.Value);
            return RoundScore;
        }

        public void AddToTotalScore()
        {
            Score += RoundScore;
        }

        public void UpdateConnectionId(string connectionId)
        {
            if (string.IsNullOrWhiteSpace(connectionId))
                throw new Exception("Connection ID cannot be empty");

            ConnectionId = connectionId;
        }
    }
}
