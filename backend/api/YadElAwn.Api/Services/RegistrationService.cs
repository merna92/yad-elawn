using Microsoft.EntityFrameworkCore;
using YadElAwn.Api.Data;
using YadElAwn.Api.Dtos;
using YadElAwn.Api.Models;
using YadElAwn.Api.Repositories;

namespace YadElAwn.Api.Services;

public record RegistrationResult(int UserId, int RelatedId);

public interface IRegistrationService
{
    Task<RegistrationResult> RegisterDonorAsync(RegisterDonorRequest request);
    Task<RegistrationResult> RegisterCharityAsync(RegisterCharityRequest request);
    Task<RegistrationResult> RegisterBeneficiaryAsync(RegisterBeneficiaryRequest request);
    Task<RegistrationResult> RegisterAdminAsync(RegisterAdminRequest request);
}

public class RegistrationService : IRegistrationService
{
    private readonly ApplicationDbContext _db;
    private readonly IUserRepository _users;
    private readonly IPasswordHasher _hasher;

    public RegistrationService(ApplicationDbContext db, IUserRepository users, IPasswordHasher hasher)
    {
        _db = db;
        _users = users;
        _hasher = hasher;
    }

    public Task<RegistrationResult> RegisterDonorAsync(RegisterDonorRequest request)
    {
        return RegisterAsync(
            request.Email,
            request.FName,
            request.LName,
            request.Password,
            request.Phone,
            request.Address,
            "Donor",
            true,
            async userId =>
            {
                var donor = new Donor { DonorId = userId, DonationCount = 0 };
                _db.Donors.Add(donor);
                await _db.SaveChangesAsync();
                return donor.DonorId;
            });
    }

    public Task<RegistrationResult> RegisterCharityAsync(RegisterCharityRequest request)
    {
        return RegisterAsync(
            request.Email,
            request.FName,
            request.LName,
            request.Password,
            request.Phone,
            request.Address,
            "Charity",
            false,
            async userId =>
            {
                var charity = new Charity
                {
                    CharityId = userId,
                    Capacity = request.Capacity,
                    LicenseNumber = request.LicenseNumber,
                    CoverageArea = request.CoverageArea,
                    Needs = request.Needs,
                    LocationId = request.LocationId
                };

                _db.Charities.Add(charity);
                await _db.SaveChangesAsync();
                return charity.CharityId;
            });
    }

    public Task<RegistrationResult> RegisterBeneficiaryAsync(RegisterBeneficiaryRequest request)
    {
        return RegisterAsync(
            request.Email,
            request.FName,
            request.LName,
            request.Password,
            request.Phone,
            request.Address,
            "Beneficiary",
            true,
            async userId =>
            {
                var beneficiary = new Beneficiary
                {
                    BeneficiaryId = userId,
                    LocationId = request.LocationId
                };

                _db.Beneficiaries.Add(beneficiary);
                await _db.SaveChangesAsync();
                return beneficiary.BeneficiaryId;
            });
    }

    public Task<RegistrationResult> RegisterAdminAsync(RegisterAdminRequest request)
    {
        return RegisterAsync(
            request.Email,
            request.FName,
            request.LName,
            request.Password,
            request.Phone,
            request.Address,
            "Admin",
            true,
            async userId =>
            {
                var admin = new Admin { AdminId = userId };
                _db.Admins.Add(admin);
                await _db.SaveChangesAsync();
                return admin.AdminId;
            });
    }

    private async Task<RegistrationResult> RegisterAsync(
        string email,
        string firstName,
        string lastName,
        string password,
        string? phone,
        string? address,
        string userType,
        bool isVerified,
        Func<int, Task<int>> createRoleRecordAsync)
    {
        if (await _users.EmailExistsAsync(email))
        {
            throw new InvalidOperationException("Email already exists.");
        }

        await using var tx = await _db.Database.BeginTransactionAsync();
        try
        {
            var user = new User
            {
                FName = firstName,
                LName = lastName,
                Email = email,
                Password = _hasher.Hash(password),
                Phone = phone,
                Address = address,
                IsVerified = isVerified,
                UserType = userType
            };

            await _users.AddAsync(user);
            await _db.SaveChangesAsync();

            var relatedId = await createRoleRecordAsync(user.UserId);
            await tx.CommitAsync();

            return new RegistrationResult(user.UserId, relatedId);
        }
        catch
        {
            await tx.RollbackAsync();
            throw;
        }
    }
}
