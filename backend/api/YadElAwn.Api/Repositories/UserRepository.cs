using Microsoft.EntityFrameworkCore;
using YadElAwn.Api.Data;
using YadElAwn.Api.Models;

namespace YadElAwn.Api.Repositories;

public interface IUserRepository
{
    Task<bool> EmailExistsAsync(string email);
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByIdAsync(int userId);
    Task AddAsync(User user);
}

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _db;

    public UserRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public Task<bool> EmailExistsAsync(string email)
    {
        return _db.Users.AnyAsync(u => u.Email == email);
    }

    public Task<User?> GetByEmailAsync(string email)
    {
        return _db.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public Task<User?> GetByIdAsync(int userId)
    {
        return _db.Users.FirstOrDefaultAsync(u => u.UserId == userId);
    }

    public Task AddAsync(User user)
    {
        _db.Users.Add(user);
        return Task.CompletedTask;
    }
}
