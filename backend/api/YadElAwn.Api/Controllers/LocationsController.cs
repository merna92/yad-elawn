using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YadElAwn.Api.Data;
using YadElAwn.Api.Dtos;
using YadElAwn.Api.Models;

namespace YadElAwn.Api.Controllers;

[ApiController]
[Route("api/locations")]
public class LocationsController : ControllerBase
{
    private readonly ApplicationDbContext _db;

    public LocationsController(ApplicationDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var locations = await _db.Locations.ToListAsync();
        return Ok(locations);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var location = await _db.Locations.FindAsync(id);
        if (location is null)
        {
            return NotFound();
        }
        return Ok(location);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateLocationRequest request)
    {
        var location = new Location
        {
            CityArea = request.CityArea,
            GpsCoordinates = request.GpsCoordinates
        };

        _db.Locations.Add(location);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = location.LocationId }, location);
    }
}
