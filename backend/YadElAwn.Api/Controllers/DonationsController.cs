using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YadElAwn.Api.Data;
using YadElAwn.Api.Dtos;
using YadElAwn.Api.Models;

namespace YadElAwn.Api.Controllers;

[ApiController]
[Route("api/donations")]
public class DonationsController : ControllerBase
{
    private readonly ApplicationDbContext _db;

    public DonationsController(ApplicationDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var donations = await _db.Donations
            .Include(d => d.Food)
            .Include(d => d.Clothes)
            .Include(d => d.Medicine)
            .Include(d => d.MedicalSupplies)
            .ToListAsync();

        return Ok(donations);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var donation = await _db.Donations
            .Include(d => d.Food)
            .Include(d => d.Clothes)
            .Include(d => d.Medicine)
            .Include(d => d.MedicalSupplies)
            .FirstOrDefaultAsync(d => d.DonationId == id);

        if (donation is null)
        {
            return NotFound();
        }

        return Ok(donation);
    }

    [HttpGet("available")]
    public async Task<IActionResult> GetAvailable()
    {
        var donations = await _db.Donations
            .Where(d => d.Status == "Available")
            .Include(d => d.Food)
            .Include(d => d.Clothes)
            .Include(d => d.Medicine)
            .Include(d => d.MedicalSupplies)
            .ToListAsync();

        return Ok(donations);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create(CreateDonationRequest request)
    {
        if (request.Food is null && request.Clothes is null && request.Medicine is null && request.MedicalSupplies is null)
        {
            return BadRequest("At least one donation type details must be provided.");
        }

        await using var tx = await _db.Database.BeginTransactionAsync();
        try
        {
            var donation = new Donation
            {
                DonorId = request.DonorId,
                LocationId = request.LocationId,
                Status = request.Status ?? "Pending",
                Image = request.Image
            };

            _db.Donations.Add(donation);
            await _db.SaveChangesAsync();

            if (request.Food is not null)
            {
                _db.Foods.Add(new Food
                {
                    DonationId = donation.DonationId,
                    ProductName = request.Food.ProductName,
                    FoodType = request.Food.FoodType,
                    Category = request.Food.Category,
                    ExpiryDate = request.Food.ExpiryDate,
                    Quantity = request.Food.Quantity,
                    StorageCondition = request.Food.StorageCondition,
                    ShelfLife = request.Food.ShelfLife
                });
            }

            if (request.Clothes is not null)
            {
                _db.Clothes.Add(new Clothes
                {
                    DonationId = donation.DonationId,
                    Season = request.Clothes.Season,
                    Gender = request.Clothes.Gender,
                    Size = request.Clothes.Size,
                    Condition = request.Clothes.Condition,
                    CleaningStatus = request.Clothes.CleaningStatus
                });
            }

            if (request.Medicine is not null)
            {
                _db.Medicines.Add(new Medicine
                {
                    DonationId = donation.DonationId,
                    MedicineName = request.Medicine.MedicineName,
                    MedicineType = request.Medicine.MedicineType,
                    ExpiryDate = request.Medicine.ExpiryDate,
                    Quantity = request.Medicine.Quantity,
                    SafetyConditions = request.Medicine.SafetyConditions
                });
            }

            if (request.MedicalSupplies is not null)
            {
                _db.MedicalSupplies.Add(new MedicalSupplies
                {
                    DonationId = donation.DonationId,
                    SupplyName = request.MedicalSupplies.SupplyName,
                    Condition = request.MedicalSupplies.Condition,
                    Quantity = request.MedicalSupplies.Quantity
                });
            }

            await _db.SaveChangesAsync();
            await tx.CommitAsync();

            return CreatedAtAction(nameof(GetById), new { id = donation.DonationId }, donation);
        }
        catch (DbUpdateException ex)
        {
            await tx.RollbackAsync();
            var message = ex.InnerException?.Message ?? ex.Message;
            return StatusCode(500, message);
        }
    }

    [Authorize]
    [HttpPatch("{id:int}/status")]
    public async Task<IActionResult> UpdateStatus(int id, UpdateDonationStatusRequest request)
    {
        var donation = await _db.Donations.FindAsync(id);
        if (donation is null)
        {
            return NotFound();
        }

        donation.Status = request.Status;
        await _db.SaveChangesAsync();

        return NoContent();
    }
}
