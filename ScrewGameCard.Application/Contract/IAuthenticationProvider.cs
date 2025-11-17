using ScrewGameCard.Domain.Entities;
using ScrewGameCard.Application.DTO.Auth;

namespace ScrewGameCard.Application.Contract;

public interface IAuthenticationProvider
{
    Task<TokenResponse> GenerateTokensAsync(Player player);
    TokenResponse GenerateTokens(Player player);
    string GenerateJwtToken(Player player);
    string GenerateRefreshToken();
    Task<bool> ValidateRefreshTokenAsync(string token);
    bool ValidateRefreshToken(string token);
    Task RevokeRefreshTokenAsync(string token);
    string HashPassword(string password);
    bool VerifyPassword(string hashedPassword, string password);
}