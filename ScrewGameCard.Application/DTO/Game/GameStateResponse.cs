using ScrewGameCard.Application.DTO.Player;

namespace ScrewGameCard.Application.DTO.Game
{
    public class GameStateResponse
    {
        public IList<PlayerStateDto> Players { get; set; }
        public int CurrentRound { get; set; }
        public string CurrentPlayerId { get; set; }
    }
}
