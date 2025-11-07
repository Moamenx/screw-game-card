using ScrewGameCard.Domain.Enums;

namespace ScrewGameCard.Contract.DTO.Game
{
    public class GameRoom
    {
        public Guid Id { get; private set; }
        public string RoomName { get; private set; }
        public List<Player.Player> Players { get; private set; }
        public GameState State { get; private set; }
        public int CurrentPlayerIndex { get; private set; }
        public int CurrentRound { get; private set; }
        public int MaxRounds { get; private set; }
        public List<Card.Card> Deck { get; private set; }
        public List<Card.Card> DiscardPile { get; private set; }
        public DateTime CreationTime { get; private set; }
        public int CardsPerPlayer { get; private set; }
        public bool HasDoublePointsRound { get; set; }

        private const int MinPlayers = 4;
        private const int MaxPlayers = 4;

        public Player.Player? CurrentPlayer => Players.ElementAtOrDefault(CurrentPlayerIndex);
        public bool IsFull => Players.Count >= MaxPlayers;
        public bool CanStart => Players.Count >= MinPlayers &&
                                Players.Count <= MaxPlayers &&
                                Players.All(p => p.IsReady) &&
                                State == GameState.Waiting;

        public GameRoom(string roomName, int maxRounds = 5, int cardsPerPlayer = 4, bool hasDoublePointsRound = true)
        {
            if (string.IsNullOrWhiteSpace(roomName))
                throw new Exception("Room name cannot be empty");

            if (maxRounds < 1)
                throw new Exception("Max rounds must be at least 1");

            if (cardsPerPlayer < 2 || cardsPerPlayer > 6)
                throw new Exception("Cards per player must be between 2 and 6");

            Id = Guid.NewGuid();
            RoomName = roomName;
            Players = [];
            State = GameState.Waiting;
            CurrentPlayerIndex = 0;
            CurrentRound = 0;
            MaxRounds = maxRounds;
            Deck = [];
            DiscardPile = [];
            CreationTime = DateTime.UtcNow;
            CardsPerPlayer = cardsPerPlayer;
            HasDoublePointsRound = hasDoublePointsRound;
        }

        public void AddPlayer(Player.Player player)
        {
            if (IsFull)
                throw new Exception("Room is full");

            if (State != GameState.Waiting)
                throw new Exception("Cannot join a game in progress");

            if (Players.Any(p => p.Name == player.Name))
                throw new Exception("Player name already exists in this room");

            Players.Add(player);
        }

        public void RemovePlayer(Player.Player player)
        {
            Players.Remove(player);
        }

        public void StartGame()
        {
            if (!CanStart)
                throw new Exception("Cannot start game. Requirements not met.");

            State = GameState.Started;
            CurrentRound = 1;
            CurrentPlayerIndex = 0;
        }

        public void SetDeck(List<Card.Card> deck)
        {
            Deck = deck ?? throw new Exception("Deck cannot be null");
        }

        public void AddToDiscardPile(Card.Card card)
        {
            DiscardPile.Add(card);
        }

        public void NextPlayer()
        {
            CurrentPlayerIndex = (CurrentPlayerIndex + 1) % Players.Count;
        }

        public void NextRound()
        {
            CurrentRound++;
            CurrentPlayerIndex = 0;
            DiscardPile.Clear();

            foreach (var player in Players)
            {
                player.AddToTotalScore();
                player.ClearHand();
                player.ResetActionUsed();
            }

            if (CurrentRound > MaxRounds)
            {
                State = GameState.Ended;
            }
            else
            {
                State = GameState.Started;
            }
        }

        public void SetInProgress()
        {
            State = GameState.InProgress;
        }

        public Player.Player? GetWinner()
        {
            if (State != GameState.Ended || Players.Count == 0)
                return null;

            return Players.OrderBy(p => p.Score).First();
        }
    }
}
