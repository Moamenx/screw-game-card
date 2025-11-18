using ScrewGameCard.Application.Contract;
using ScrewGameCard.Application.DTO.Auth;
using ScrewGameCard.Application.DTO.Player;
using ScrewGameCard.Application.Repositories;
using ScrewGameCard.Domain.Entities;
using ScrewGameCard.DomainShared.Constants;
using Microsoft.Extensions.Logging;
using ScrewGameCard.DomainShared.Exceptions;
using ScrewGameCard.DomainShared;

namespace ScrewGameCard.Application.Service;

public class AuthService : IAuthService
{
    private readonly IGenericRepository<Player> _playerRepository;
    private readonly IGenericRepository<RefreshToken> _refreshTokenRepository;
    private readonly IAuthenticationProvider _authProvider;
    private readonly ILogger<AuthService> _logger;
    private readonly ILocalizationService _localizationService;

    public AuthService(IGenericRepository<Player> playerRepository, IGenericRepository<RefreshToken> refreshTokenRepository, IAuthenticationProvider authProvider, ILogger<AuthService> logger, ILocalizationService localizationService)
    {
        _playerRepository = playerRepository ?? throw new ArgumentNullException(nameof(playerRepository));
        _refreshTokenRepository = refreshTokenRepository ?? throw new ArgumentNullException(nameof(refreshTokenRepository));
        _authProvider = authProvider ?? throw new ArgumentNullException(nameof(authProvider));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _localizationService = localizationService ?? throw new ArgumentNullException(nameof(localizationService));
    }

    public async Task<string> RegisterAsync(RegisterRequest request)
    {
        if (await _playerRepository.ExistsAsync(p => p.Name == request.Username))
        {
            var message = await _localizationService.GetMessageAsync("UsernameAlreadyExists");
            throw new BadRequestException(message, DomainErrorCodes.UsernameAlreadyExists);
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
            var message = await _localizationService.GetMessageAsync("InvalidCredentials");
            throw new UnauthorizedException(message, DomainErrorCodes.InvalidCredentials);
        }
         
        if (!_authProvider.VerifyPassword(player.Password, request.Password))
        {
            var message = await _localizationService.GetMessageAsync("InvalidCredentials");
            throw new UnauthorizedException(message, DomainErrorCodes.InvalidCredentials);
        }
        
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
            var message = await _localizationService.GetMessageAsync("InvalidRefreshToken");
            throw new UnauthorizedException(message, DomainErrorCodes.InvalidRefreshToken);
        }

        var refreshTokenEntity = await _refreshTokenRepository.FirstOrDefaultAsync(rt => rt.Token == refreshToken);
        if (refreshTokenEntity == null)
        {
            var message = await _localizationService.GetMessageAsync("InvalidRefreshToken");
            throw new UnauthorizedException(message, DomainErrorCodes.InvalidRefreshToken);
        }

        var player = await _playerRepository.GetAsync(refreshTokenEntity.PlayerId);
        if (player == null)
        {
            var message = await _localizationService.GetMessageAsync("PlayerNotFound");
            throw new UnauthorizedException(message, DomainErrorCodes.PlayerNotFound);
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
}