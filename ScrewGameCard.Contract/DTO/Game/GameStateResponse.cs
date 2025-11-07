using ScrewGameCard.Contract.DTO.Player;

namespace ScrewGameCard.Contract.DTO.Game
{
    public class GameStateResponse
    {
        public IList<PlayerStateDto> Players { get; set; }
        public int CurrentRound { get; set; }
        public string CurrentPlayerId { get; set; }
    }
}
