using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using YadElAwn.Api.Data;
using YadElAwn.Api.Dtos;
using YadElAwn.Api.Services;

namespace YadElAwn.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly ApplicationDbContext _db;
    private readonly IConfiguration _config;
    private readonly IPasswordHasher _hasher;

    public AuthController(ApplicationDbContext db, IConfiguration config, IPasswordHasher hasher)
    {
        _db = db;
        _config = config;
        _hasher = hasher;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
        if (user is null)
        {
            return Unauthorized("Invalid credentials.");
        }

        if (!_hasher.Verify(request.Password, user.Password))
        {
            return Unauthorized("Invalid credentials.");
        }

        var token = GenerateJwt(user.UserId, user.Email, user.UserType);

        var response = new AuthResponse
        {
            UserId = user.UserId,
            Email = user.Email,
            UserType = user.UserType,
            Token = token
        };

        return Ok(response);
    }

    private string GenerateJwt(int userId, string email, string? userType)
    {
        var jwt = _config.GetSection("Jwt");
        var key = jwt["Key"] ?? throw new InvalidOperationException("Jwt:Key is missing.");
        var issuer = jwt["Issuer"] ?? "YadElAwn";
        var audience = jwt["Audience"] ?? "YadElAwn";

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, email),
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim(ClaimTypes.Email, email)
        };

        if (!string.IsNullOrWhiteSpace(userType))
        {
            claims.Add(new Claim(ClaimTypes.Role, userType));
        }

        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var creds = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(6),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
