using ScrewGameCard.Application.DTO.Auth;
using ScrewGameCard.Domain.Entities;

namespace ScrewGameCard.Application.Contract;

public interface IAuthService
{
    Task<string> RegisterAsync(RegisterRequest request);
    Task<Player> LoginAsync(LoginRequest request);
}