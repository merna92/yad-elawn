using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YadElAwn.Api.Data;
using YadElAwn.Api.Dtos;
using YadElAwn.Api.Models;

namespace YadElAwn.Api.Controllers;

[ApiController]
[Route("api/notifications")]
public class NotificationsController : ControllerBase
{
    private readonly ApplicationDbContext _db;

    public NotificationsController(ApplicationDbContext db)
    {
        _db = db;
    }

    [Authorize]
    [HttpGet("{userId:int}")]
    public async Task<IActionResult> GetByUser(int userId)
    {
        var notifications = await _db.Notifications
            .Where(n => n.UserId == userId)
            .OrderByDescending(n => n.Timestamp)
            .ToListAsync();

        return Ok(notifications);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create(CreateNotificationRequest request)
    {
        var notification = new Notification
        {
            UserId = request.UserId,
            Content = request.Content,
            Timestamp = DateTime.UtcNow,
            IsRead = false
        };

        _db.Notifications.Add(notification);
        await _db.SaveChangesAsync();

        return Ok(notification);
    }
}
