using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YadElAwn.Api.Data;
using YadElAwn.Api.Dtos;
using YadElAwn.Api.Models;
using YadElAwn.Api.Services;

namespace YadElAwn.Api.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly ApplicationDbContext _db;
    private readonly IPasswordHasher _hasher;

    public UsersController(ApplicationDbContext db, IPasswordHasher hasher)
    {
        _db = db;
        _hasher = hasher;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var users = await _db.Users
            .Select(u => new
            {
                u.UserId,
                u.FName,
                u.LName,
                u.Email,
                u.Phone,
                u.Address,
                u.IsVerified,
                u.UserType
            })
            .ToListAsync();

        return Ok(users);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var user = await _db.Users
            .Where(u => u.UserId == id)
            .Select(u => new
            {
                u.UserId,
                u.FName,
                u.LName,
                u.Email,
                u.Phone,
                u.Address,
                u.IsVerified,
                u.UserType
            })
            .FirstOrDefaultAsync();

        if (user is null)
        {
            return NotFound();
        }

        return Ok(user);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateUserRequest request)
    {
        var exists = await _db.Users.AnyAsync(u => u.Email == request.Email);
        if (exists)
        {
            return Conflict("Email already exists.");
        }

        var user = new User
        {
            FName = request.FName,
            LName = request.LName,
            Email = request.Email,
            Password = _hasher.Hash(request.Password),
            Phone = request.Phone,
            Address = request.Address,
            IsVerified = request.IsVerified,
            UserType = request.UserType
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = user.UserId }, new
        {
            user.UserId,
            user.FName,
            user.LName,
            user.Email,
            user.Phone,
            user.Address,
            user.IsVerified,
            user.UserType
        });
    }
}
