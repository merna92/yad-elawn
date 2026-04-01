using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YadElAwn.Api.Data;
using YadElAwn.Api.Dtos;
using YadElAwn.Api.Models;

namespace YadElAwn.Api.Controllers;

[ApiController]
[Route("api/messages")]
public class MessagesController : ControllerBase
{
    private readonly ApplicationDbContext _db;

    public MessagesController(ApplicationDbContext db)
    {
        _db = db;
    }

    [Authorize]
    [HttpGet("inbox/{userId:int}")]
    public async Task<IActionResult> GetInbox(int userId)
    {
        var messages = await _db.Messages
            .Where(m => m.ReceiverId == userId)
            .OrderByDescending(m => m.SentDateTime)
            .ToListAsync();

        return Ok(messages);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create(CreateMessageRequest request)
    {
        var message = new Message
        {
            SenderId = request.SenderId,
            ReceiverId = request.ReceiverId,
            Content = request.Content,
            SentDateTime = DateTime.UtcNow,
            IsRead = false
        };

        _db.Messages.Add(message);
        await _db.SaveChangesAsync();

        return Ok(message);
    }
}
