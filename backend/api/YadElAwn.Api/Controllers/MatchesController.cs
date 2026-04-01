using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YadElAwn.Api.Data;
using YadElAwn.Api.Dtos;
using YadElAwn.Api.Models;

namespace YadElAwn.Api.Controllers;

[ApiController]
[Route("api/matches")]
public class MatchesController : ControllerBase
{
    private readonly ApplicationDbContext _db;

    public MatchesController(ApplicationDbContext db)
    {
        _db = db;
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var matches = await _db.Matches.ToListAsync();
        return Ok(matches);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create(CreateMatchRequest request)
    {
        var match = new Match
        {
            DonationId = request.DonationId,
            CharityId = request.CharityId,
            BeneficiaryId = request.BeneficiaryId,
            Distance = request.Distance,
            UrgencyLevel = request.UrgencyLevel,
            MatchDate = DateTime.UtcNow
        };

        _db.Matches.Add(match);
        await _db.SaveChangesAsync();

        return Ok(match);
    }
}
