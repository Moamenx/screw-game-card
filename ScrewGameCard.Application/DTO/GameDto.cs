using ScrewGameCard.Domain.Enums;

namespace ScrewGameCard.Application.DTO
{
    public class GameDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public GameStatus Status { get; set; }
    
    }
}
