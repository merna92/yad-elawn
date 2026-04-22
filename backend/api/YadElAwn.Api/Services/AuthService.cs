using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using YadElAwn.Api.Dtos;
using YadElAwn.Api.Repositories;

namespace YadElAwn.Api.Services;

public interface IAuthService
{
    Task<AuthResponse?> LoginAsync(LoginRequest request);
}

public class AuthService : IAuthService
{
    private readonly IUserRepository _users;
    private readonly IConfiguration _config;
    private readonly IPasswordHasher _hasher;

    public AuthService(IUserRepository users, IConfiguration config, IPasswordHasher hasher)
    {
        _users = users;
        _config = config;
        _hasher = hasher;
    }

    public async Task<AuthResponse?> LoginAsync(LoginRequest request)
    {
        var user = await _users.GetByEmailAsync(request.Email);
        if (user is null)
        {
            return null;
        }

        if (!_hasher.Verify(request.Password, user.Password))
        {
            return null;
        }

        return new AuthResponse
        {
            UserId = user.UserId,
            Email = user.Email,
            UserType = user.UserType,
            Token = GenerateJwt(user.UserId, user.Email, user.UserType)
        };
    }

    private string GenerateJwt(int userId, string email, string? userType)
    {
        var jwt = _config.GetSection("Jwt");
        var key = jwt["Key"] ?? throw new InvalidOperationException("Jwt:Key is missing.");
        var issuer = jwt["Issuer"] ?? "YadElAwn";
        var audience = jwt["Audience"] ?? "YadElAwn";

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new(JwtRegisteredClaimNames.Email, email),
            new(ClaimTypes.NameIdentifier, userId.ToString()),
            new(ClaimTypes.Email, email)
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
