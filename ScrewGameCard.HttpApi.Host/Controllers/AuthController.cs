using Microsoft.AspNetCore.Mvc;
using ScrewGameCard.Application.Contract;
using ScrewGameCard.Application.DTO.Auth;
using ScrewGameCard.DomainShared;
using ScrewGameCard.HttpApi.Host.Filters;

namespace ScrewGameCard.HttpApi.Host.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService ?? throw new ArgumentNullException(nameof(authService));
    }

    [HttpPost("register")]
    [RateLimit("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var result = await _authService.RegisterAsync(request);
        var correlationId = HttpContext.Request.Headers["X-Request-Id"].FirstOrDefault();
        return Ok(ApiResponse<string>.Success(result, correlationId: correlationId));
    }

    [HttpPost("login")]
    [RateLimit("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var response = await _authService.LoginAsync(request);
        var correlationId = HttpContext.Request.Headers["X-Request-Id"].FirstOrDefault();
        return Ok(ApiResponse<LoginResponse>.Success(response, correlationId: correlationId));
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request)
    {
        var response = await _authService.RefreshAsync(request.RefreshToken);
        var correlationId = HttpContext.Request.Headers["X-Request-Id"].FirstOrDefault();
        return Ok(ApiResponse<LoginResponse>.Success(response, correlationId: correlationId));
    }
}

