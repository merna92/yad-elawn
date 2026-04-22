using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YadElAwn.Api.Dtos;
using YadElAwn.Api.Services;

namespace YadElAwn.Api.Controllers;

[ApiController]
[Route("api/donations")]
public class DonationsController : ControllerBase
{
    private readonly IDonationService _donations;

    public DonationsController(IDonationService donations)
    {
        _donations = donations;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _donations.GetAllAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var donation = await _donations.GetByIdAsync(id);
        if (donation is null)
        {
            return NotFound();
        }

        return Ok(donation);
    }

    [HttpGet("available")]
    public async Task<IActionResult> GetAvailable()
    {
        return Ok(await _donations.GetAvailableAsync());
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create(CreateDonationRequest request)
    {
        try
        {
            var donation = await _donations.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = donation.DonationId }, donation);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [Authorize]
    [HttpPatch("{id:int}/status")]
    public async Task<IActionResult> UpdateStatus(int id, UpdateDonationStatusRequest request)
    {
        var updated = await _donations.UpdateStatusAsync(id, request.Status);
        if (!updated)
        {
            return NotFound();
        }

        return NoContent();
    }
}
