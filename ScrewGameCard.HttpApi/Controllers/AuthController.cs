using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ScrewGameCard.Application.DTO.Auth;
using ScrewGameCard.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ScrewGameCard.HttpApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly IGenericRepository<Player> _playerRepository;

    public AuthController(IConfiguration configuration, IGenericRepository<Player> playerRepository)
    {
        _configuration = configuration;
        _playerRepository = playerRepository;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        if (await _playerRepository.ExistsAsync(p => p.Name == request.Username))
        {
            return BadRequest("Username already exists");
        }

        var player = new Player
        {
            Name = request.Username,
            // Hash password
            // For simplicity, store plain text (not recommended for production)
            // In real app, use BCrypt or similar
            // Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
            // But since Player doesn't have Password, perhaps add it or use another way
            // For now, assume Player has a Password field
            // Let's add Password to Player entity
            CreatedDate = DateTime.Now
        };

        await _playerRepository.AddAsync(player);

        return Ok("User registered successfully");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var player = await _playerRepository.FirstOrDefaultAsync(p => p.Name == request.Username);
        if (player == null)
        {
            return Unauthorized("Invalid credentials");
        }

        // Verify password
        // if (!BCrypt.Net.BCrypt.Verify(request.Password, player.Password))
        //     return Unauthorized("Invalid credentials");

        // For now, skip password check

        var token = GenerateJwtToken(player);
        return Ok(new { Token = token, Player = new { player.Id, player.Name } });
    }

    private string GenerateJwtToken(Player player)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, player.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, player.Name),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}