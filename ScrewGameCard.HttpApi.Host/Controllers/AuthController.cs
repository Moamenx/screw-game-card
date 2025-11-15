using Microsoft.AspNetCore.Mvc;
using ScrewGameCard.Application.Contract;
using ScrewGameCard.Application.DTO.Auth;
using ScrewGameCard.Domain.Entities;

namespace ScrewGameCard.HttpApi.Host.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IAuthenticationProvider _authProvider;

    public AuthController(IAuthService authService, IAuthenticationProvider authProvider)
    {
        _authService = authService ?? throw new ArgumentNullException(nameof(authService));
        _authProvider = authProvider ?? throw new ArgumentNullException(nameof(authProvider));
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        try
        {
            var result = await _authService.RegisterAsync(request);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        try
        {
            var player = await _authService.LoginAsync(request);
            var (accessToken, refreshToken) = await _authProvider.GenerateTokensAsync(player);
            return Ok(new { AccessToken = accessToken, RefreshToken = refreshToken, Player = new { player.Id, player.Name } });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(ex.Message);
        }
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshRequest request)
    {
        if (!await _authProvider.ValidateRefreshTokenAsync(request.RefreshToken))
        {
            return Unauthorized("Invalid refresh token");
        }

        // In production, decode refresh token to get user ID
        // For simplicity, assume we need to re-authenticate or store user in token
        // Here, we'll require re-login for now
        return BadRequest("Refresh token validation requires user context. Please re-login.");
    }
}

public class RefreshRequest
{
    public string RefreshToken { get; set; }
}