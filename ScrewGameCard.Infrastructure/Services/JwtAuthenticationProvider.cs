using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ScrewGameCard.Application.Contract;
using ScrewGameCard.Application.DTO.Auth;
using ScrewGameCard.Application.Repositories;
using ScrewGameCard.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Logging;

namespace ScrewGameCard.Infrastructure.Services;

public class JwtAuthenticationProvider : IAuthenticationProvider
{
    private readonly IConfiguration _configuration;
    private readonly IGenericRepository<RefreshToken> _refreshTokenRepository;
    private readonly ILogger<JwtAuthenticationProvider> _logger;

    public JwtAuthenticationProvider(IConfiguration configuration, IGenericRepository<RefreshToken> refreshTokenRepository, ILogger<JwtAuthenticationProvider> logger)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _refreshTokenRepository = refreshTokenRepository ?? throw new ArgumentNullException(nameof(refreshTokenRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<TokenResponse> GenerateTokensAsync(Player player)
    {
        var accessToken = GenerateJwtToken(player);
        var refreshToken = GenerateRefreshToken();

        var refreshTokenEntity = new RefreshToken
        {
            Token = refreshToken,
            PlayerId = player.Id,
            ExpiresAt = DateTime.Now.AddDays(30),
            IsRevoked = false
        };

        await _refreshTokenRepository.AddAsync(refreshTokenEntity);
        _logger.LogInformation("Tokens generated for user {UserName}", player.Name);

        return new TokenResponse { AccessToken = accessToken, RefreshToken = refreshToken, ExpiresIn = 900 };
    }

    public TokenResponse GenerateTokens(Player player)
    {
        return GenerateTokensAsync(player).GetAwaiter().GetResult();
    }

    public string GenerateJwtToken(Player player)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, player.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, player.Name),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(15), // Shorter access token
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        var randomBytes = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomBytes);
        }
        return Convert.ToBase64String(randomBytes);
    }

    public async Task<bool> ValidateRefreshTokenAsync(string token)
    {
        var refreshToken = await _refreshTokenRepository.FirstOrDefaultAsync(rt => rt.Token == token && !rt.IsRevoked && rt.ExpiresAt > DateTime.Now);
        var isValid = refreshToken != null;
        _logger.LogInformation("Refresh token validation: {IsValid} for token {TokenHash}", isValid, HashTokenForLogging(token));
        return isValid;
    }

    public bool ValidateRefreshToken(string token)
    {
        return Task.Run(() => ValidateRefreshTokenAsync(token)).GetAwaiter().GetResult();
    }

    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public async Task RevokeRefreshTokenAsync(string token)
    {
        var refreshToken = await _refreshTokenRepository.FirstOrDefaultAsync(rt => rt.Token == token);
        if (refreshToken != null)
        {
            refreshToken.IsRevoked = true;
            await _refreshTokenRepository.UpdateAsync(refreshToken);
            _logger.LogInformation("Refresh token revoked for token {TokenHash}", HashTokenForLogging(token));
        }
        else
        {
            _logger.LogWarning("Attempted to revoke non-existent refresh token {TokenHash}", HashTokenForLogging(token));
        }
    }

    public bool VerifyPassword(string hashedPassword, string password)
    {
        return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }

    private string HashTokenForLogging(string token)
    {
        using var sha256 = System.Security.Cryptography.SHA256.Create();
        var hash = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(token));
        return Convert.ToBase64String(hash);
    }
}


































































































