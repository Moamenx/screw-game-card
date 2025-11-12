using ScrewGameCard.Application.DTO;
using ScrewGameCard.Domain.Entities;

namespace ScrewGameCard.Application.Mapping
{
    public static class GameDomainToDtoMapper
    {
        public static GameDto ToGameDto(this Game game)
        {
            return new GameDto
            {
                Id = game.Id,
                Name = game.Name,
                Status = game.Status
            };
        }
    }
}
