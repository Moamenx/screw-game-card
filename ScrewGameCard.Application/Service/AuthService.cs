using ScrewGameCard.Application.Contract;
using ScrewGameCard.Application.DTO.Auth;
using ScrewGameCard.Application.DTO.Player;
using ScrewGameCard.Application.Repository;
using ScrewGameCard.Domain.Entities;
using ScrewGameCard.DomainShared.Constants;
using Microsoft.Extensions.Logging;
using ScrewGameCard.DomainShared.Exceptions;

namespace ScrewGameCard.Application.Service;

public class AuthService : IAuthService
{
    private readonly IGenericRepository<Player> _playerRepository;
    private readonly IGenericRepository<RefreshToken> _refreshTokenRepository;
    private readonly IAuthenticationProvider _authProvider;
    private readonly ILogger<AuthService> _logger;

    public AuthService(IGenericRepository<Player> playerRepository, IGenericRepository<RefreshToken> refreshTokenRepository, IAuthenticationProvider authProvider, ILogger<AuthService> logger)
    {
        _playerRepository = playerRepository ?? throw new ArgumentNullException(nameof(playerRepository));
        _refreshTokenRepository = refreshTokenRepository ?? throw new ArgumentNullException(nameof(refreshTokenRepository));
        _authProvider = authProvider ?? throw new ArgumentNullException(nameof(authProvider));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<string> RegisterAsync(RegisterRequest request)
    {
        if (await _playerRepository.ExistsAsync(p => p.Name == request.Username))
        {
            _logger.LogWarning("Registration failed: Username {UserName} already exists", request.Username);
            throw new BadRequestException("Username already exists");
        }

        var hashedPassword = _authProvider.HashPassword(request.Password);

        var player = new Player
        {
            Id = Guid.CreateVersion7(),
            Name = request.Username,
            AvatarUrl = "dummyurl",
            Language = Global.Language.DefaultLanguage,
            Password = hashedPassword,
            CreatedDate = DateTime.Now
        };

        var playerEntity = await _playerRepository.AddAsync(player);
        _logger.LogInformation("User {UserName} registered successfully with ID {UserId}", request.Username, playerEntity.Id);

        return playerEntity.Id.ToString();
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        var player = await _playerRepository.FirstOrDefaultAsync(p => p.Name == request.Username);
        if (player == null)
        {
            throw new UnauthorizedException("Invalid credentials");
        }
         
        if (!_authProvider.VerifyPassword(player.Password, request.Password))       
            throw new UnauthorizedException("Invalid credentials");
        
        _logger.LogInformation("User {UserName} logged in successfully", request.Username);

        var tokenResponse = await _authProvider.GenerateTokensAsync(player);

        return new LoginResponse
        {
            AccessToken = tokenResponse.AccessToken,
            RefreshToken = tokenResponse.RefreshToken,
            ExpiresIn = tokenResponse.ExpiresIn,
            Player = new PlayerDto { Id = player.Id, Name = player.Name }
        };
    }

    public async Task<LoginResponse> RefreshAsync(string refreshToken)
    {
        if (!await _authProvider.ValidateRefreshTokenAsync(refreshToken))
        {
            _logger.LogWarning("Refresh token validation failed for token {TokenHash}", HashTokenForLogging(refreshToken));
            throw new UnauthorizedException("Invalid refresh token");
        }

        // Get the refresh token entity to find the player
        var refreshTokenEntity = await _refreshTokenRepository.FirstOrDefaultAsync(rt => rt.Token == refreshToken);
        if (refreshTokenEntity == null)
        {
            _logger.LogWarning("Refresh token entity not found for token {TokenHash}", HashTokenForLogging(refreshToken));
            throw new UnauthorizedException("Invalid refresh token");
        }

        // Get the player
        var player = await _playerRepository.GetAsync(refreshTokenEntity.PlayerId);
        if (player == null)
        {
            _logger.LogWarning("Player not found for refresh token {TokenHash}, PlayerId {PlayerId}", HashTokenForLogging(refreshToken), refreshTokenEntity.PlayerId);
            throw new UnauthorizedException("Player not found");
        }

        // Revoke the old refresh token
        await _authProvider.RevokeRefreshTokenAsync(refreshToken);
        _logger.LogInformation("Old refresh token revoked for user {UserName}", player.Name);

        // Generate new tokens
        var tokenResponse = await _authProvider.GenerateTokensAsync(player);
        _logger.LogInformation("New tokens generated for user {UserName}", player.Name);

        return new LoginResponse
        {
            AccessToken = tokenResponse.AccessToken,
            RefreshToken = tokenResponse.RefreshToken,
            ExpiresIn = tokenResponse.ExpiresIn,
            Player = new PlayerDto { Id = player.Id, Name = player.Name }
        };
    }

    private string HashTokenForLogging(string token)
    {
        using var sha256 = System.Security.Cryptography.SHA256.Create();
        var hash = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(token));
        return Convert.ToBase64String(hash);
    }
}