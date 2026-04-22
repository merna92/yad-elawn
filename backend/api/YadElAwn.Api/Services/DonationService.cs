using YadElAwn.Api.Data;
using YadElAwn.Api.Dtos;
using YadElAwn.Api.Models;
using YadElAwn.Api.Repositories;

namespace YadElAwn.Api.Services;

public interface IDonationService
{
    Task<List<Donation>> GetAllAsync();
    Task<List<Donation>> GetAvailableAsync();
    Task<Donation?> GetByIdAsync(int id);
    Task<Donation> CreateAsync(CreateDonationRequest request);
    Task<bool> UpdateStatusAsync(int id, string status);
}

public class DonationService : IDonationService
{
    private readonly ApplicationDbContext _db;
    private readonly IDonationRepository _donations;

    public DonationService(ApplicationDbContext db, IDonationRepository donations)
    {
        _db = db;
        _donations = donations;
    }

    public Task<List<Donation>> GetAllAsync()
    {
        return _donations.GetAllAsync();
    }

    public Task<List<Donation>> GetAvailableAsync()
    {
        return _donations.GetAvailableAsync();
    }

    public Task<Donation?> GetByIdAsync(int id)
    {
        return _donations.GetByIdAsync(id);
    }

    public async Task<Donation> CreateAsync(CreateDonationRequest request)
    {
        if (request.Food is null && request.Clothes is null && request.Medicine is null && request.MedicalSupplies is null)
        {
            throw new InvalidOperationException("At least one donation type details must be provided.");
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

            await _donations.AddAsync(donation);
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

            return donation;
        }
        catch
        {
            await tx.RollbackAsync();
            throw;
        }
    }

    public async Task<bool> UpdateStatusAsync(int id, string status)
    {
        var donation = await _db.Donations.FindAsync(id);
        if (donation is null)
        {
            return false;
        }

        donation.Status = status;
        await _db.SaveChangesAsync();
        return true;
    }
}
