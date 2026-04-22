using Microsoft.AspNetCore.Mvc;
using YadElAwn.Api.Dtos;
using YadElAwn.Api.Services;

namespace YadElAwn.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var response = await _authService.LoginAsync(request);
        if (response is null)
        {
            return Unauthorized("Invalid credentials.");
        }

        return Ok(response);
    }
}
