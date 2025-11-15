using ScrewGameCard.Domain.Entities;

namespace ScrewGameCard.Application.Contract;

public interface IAuthenticationProvider
{
    Task<(string AccessToken, string RefreshToken)> GenerateTokensAsync(Player player);
    (string AccessToken, string RefreshToken) GenerateTokens(Player player);
    string GenerateJwtToken(Player player);
    string GenerateRefreshToken();
    Task<bool> ValidateRefreshTokenAsync(string token);
    bool ValidateRefreshToken(string token);
    Task RevokeRefreshTokenAsync(string token);
    string HashPassword(string password);
    bool VerifyPassword(string hashedPassword, string password);
}