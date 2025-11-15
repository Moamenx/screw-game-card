using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ScrewGameCard.Application.Contract;
using ScrewGameCard.Application.Repository;
using ScrewGameCard.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ScrewGameCard.Infrastructure.Services;

public class JwtAuthenticationProvider : IAuthenticationProvider
{
    private readonly IConfiguration _configuration;
    private readonly IGenericRepository<RefreshToken> _refreshTokenRepository;

    public JwtAuthenticationProvider(IConfiguration configuration, IGenericRepository<RefreshToken> refreshTokenRepository)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _refreshTokenRepository = refreshTokenRepository ?? throw new ArgumentNullException(nameof(refreshTokenRepository));
    }

    public async Task<(string AccessToken, string RefreshToken)> GenerateTokensAsync(Player player)
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

        return (accessToken, refreshToken);
    }

    public (string AccessToken, string RefreshToken) GenerateTokens(Player player)
    {
        return Task.Run(() => GenerateTokensAsync(player)).Result;
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
        return refreshToken != null;
    }

    public bool ValidateRefreshToken(string token)
    {
        return Task.Run(() => ValidateRefreshTokenAsync(token)).Result;
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
        }
    }

    public bool VerifyPassword(string hashedPassword, string password)
    {
        return BCrypt.Net.BCrypt.Verify(password,hashedPassword);
    }
}

