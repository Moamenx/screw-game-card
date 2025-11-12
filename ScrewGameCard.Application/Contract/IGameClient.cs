using ScrewGameCard.Application.DTO;
using ScrewGameCard.Application.DTO.CreateRoom;

namespace ScrewGameCard.Application.Contract
{
    public interface IGameClient
    {
        public Task GameCreated(GameDto game);
        public Task Error(string message);
    }
}
