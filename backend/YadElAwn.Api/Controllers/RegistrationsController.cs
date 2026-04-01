using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YadElAwn.Api.Data;
using YadElAwn.Api.Dtos;
using YadElAwn.Api.Models;
using YadElAwn.Api.Services;

namespace YadElAwn.Api.Controllers;

[ApiController]
[Route("api/registrations")]
public class RegistrationsController : ControllerBase
{
    private readonly ApplicationDbContext _db;
    private readonly IPasswordHasher _hasher;

    public RegistrationsController(ApplicationDbContext db, IPasswordHasher hasher)
    {
        _db = db;
        _hasher = hasher;
    }

    [HttpPost("donor")]
    public async Task<IActionResult> RegisterDonor(RegisterDonorRequest request)
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
            IsVerified = true,
            UserType = "Donor"
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        var donor = new Donor
        {
            DonorId = user.UserId,
            DonationCount = 0
        };

        _db.Donors.Add(donor);
        await _db.SaveChangesAsync();

        return Ok(new { user.UserId, donor.DonorId });
    }

    [HttpPost("charity")]
    public async Task<IActionResult> RegisterCharity(RegisterCharityRequest request)
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
            IsVerified = false,
            UserType = "Charity"
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        var charity = new Charity
        {
            CharityId = user.UserId,
            Capacity = request.Capacity,
            LicenseNumber = request.LicenseNumber,
            CoverageArea = request.CoverageArea,
            Needs = request.Needs,
            LocationId = request.LocationId
        };

        _db.Charities.Add(charity);
        await _db.SaveChangesAsync();

        return Ok(new { user.UserId, charity.CharityId });
    }

    [HttpPost("beneficiary")]
    public async Task<IActionResult> RegisterBeneficiary(RegisterBeneficiaryRequest request)
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
            IsVerified = true,
            UserType = "Beneficiary"
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        var beneficiary = new Beneficiary
        {
            BeneficiaryId = user.UserId,
            LocationId = request.LocationId
        };

        _db.Beneficiaries.Add(beneficiary);
        await _db.SaveChangesAsync();

        return Ok(new { user.UserId, beneficiary.BeneficiaryId });
    }

    [HttpPost("admin")]
    public async Task<IActionResult> RegisterAdmin(RegisterAdminRequest request)
    {
        var exists = await _db.Users.AnyAsync(u => u.Email == request.Email);
        if (exists)
        {
            return Conflict("Email already exists.");
        }

        await using var tx = await _db.Database.BeginTransactionAsync();
        try
        {
            var user = new User
            {
                FName = request.FName,
                LName = request.LName,
                Email = request.Email,
                Password = _hasher.Hash(request.Password),
                Phone = request.Phone,
                Address = request.Address,
                IsVerified = true,
                UserType = "Admin"
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            var admin = new Admin
            {
                AdminId = user.UserId
            };

            _db.Admins.Add(admin);
            await _db.SaveChangesAsync();

            await tx.CommitAsync();

            return Ok(new { user.UserId, admin.AdminId });
        }
        catch (DbUpdateException ex)
        {
            await tx.RollbackAsync();
            var message = ex.InnerException?.Message ?? ex.Message;
            return StatusCode(500, message);
        }
    }
}
