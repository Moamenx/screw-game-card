using ScrewGameCard.Domain.Enums;

namespace ScrewGameCard.Contract.DTO
{
    public class GameRoomDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public GameState State { get; set; }
    }
}
