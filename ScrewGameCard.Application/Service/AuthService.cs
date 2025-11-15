using ScrewGameCard.Application.Contract;
using ScrewGameCard.Application.DTO.Auth;
using ScrewGameCard.Application.Repository;
using ScrewGameCard.Domain.Entities;

namespace ScrewGameCard.Application.Service;

public class AuthService : IAuthService
{
    private readonly IGenericRepository<Player> _playerRepository;
    private readonly IAuthenticationProvider _authProvider;

    public AuthService(IGenericRepository<Player> playerRepository, IAuthenticationProvider authProvider)
    {
        _playerRepository = playerRepository ?? throw new ArgumentNullException(nameof(playerRepository));
        _authProvider = authProvider ?? throw new ArgumentNullException(nameof(authProvider));
    }

    public async Task<string> RegisterAsync(RegisterRequest request)
    {
        if (await _playerRepository.ExistsAsync(p => p.Name == request.Username))
        {
            throw new InvalidOperationException("Username already exists");
        }

        var hashedPassword = _authProvider.HashPassword(request.Password);

        var player = new Player
        {
            Name = request.Username,
            Password = hashedPassword,
            CreatedDate = DateTime.Now
        };

        await _playerRepository.AddAsync(player);

        return "User registered successfully";
    }

    public async Task<Player> LoginAsync(LoginRequest request)
    {
        var player = await _playerRepository.FirstOrDefaultAsync(p => p.Name == request.Username);
        if (player == null)
        {
            throw new UnauthorizedAccessException("Invalid credentials");
        }

        if (!_authProvider.VerifyPassword(player.Password, request.Password))
        {
            throw new UnauthorizedAccessException("Invalid credentials");
        }

        return player;
    }
}