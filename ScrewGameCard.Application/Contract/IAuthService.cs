using ScrewGameCard.Application.DTO.Auth;
using ScrewGameCard.Domain.Entities;

namespace ScrewGameCard.Application.Contract;

public interface IAuthService
{
    Task<string> RegisterAsync(RegisterRequest request);
    Task<LoginResponse> LoginAsync(LoginRequest request);
    Task<LoginResponse> RefreshAsync(string refreshToken);
}