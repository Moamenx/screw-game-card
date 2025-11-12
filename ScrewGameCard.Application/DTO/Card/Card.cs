using ScrewGameCard.Domain.Enums;

namespace ScrewGameCard.Application.DTO.Card
{
    public class Card
    {
        public string Id { get; private set; }
        public CardType Type { get; private set; }
        public int Value { get; private set; }
        public bool IsRevealed { get; private set; }

        public Card(CardType type, int value)
        {
            Id = Guid.NewGuid().ToString();
            Type = type;
            Value = value;
            IsRevealed = false;
        }

        private Card(string id, CardType type, int value, bool isRevealed)
        {
            Id = id;
            Type = type;
            Value = value;
            IsRevealed = isRevealed;
        }

        public void Reveal()
        {
            IsRevealed = true;
        }

        public void Hide()
        {
            IsRevealed = false;
        }

        public Card Clone()
        {
            return new Card(Id, Type, Value, IsRevealed);
        }

        public bool IsActionCard()
        {
            return Type is CardType.SeeAndSwap or CardType.BlindSwap
                or CardType.PeekOwn or CardType.PeekOther or CardType.MatchingCard;
        }
    }
}
