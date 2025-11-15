using ScrewGameCard.Application.DTO.Player;

namespace ScrewGameCard.Application.DTO.Auth;

public class RegisterRequest
{
    public string Username { get; set; }
    public string Password { get; set; }
}

public class LoginRequest
{
    public string Username { get; set; }
    public string Password { get; set; }
}

public class AuthResponse
{
    public string Token { get; set; }
    public PlayerDto Player { get; set; }
}