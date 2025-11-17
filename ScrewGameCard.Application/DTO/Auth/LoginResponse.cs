using ScrewGameCard.Domain.Entities;
using ScrewGameCard.Application.DTO.Player;

namespace ScrewGameCard.Application.DTO.Auth
{
    public class LoginResponse
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public int ExpiresIn { get; set; }
        public PlayerDto Player { get; set; }
    }
}