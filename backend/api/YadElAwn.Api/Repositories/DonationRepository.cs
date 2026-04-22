using Microsoft.EntityFrameworkCore;
using YadElAwn.Api.Data;
using YadElAwn.Api.Models;

namespace YadElAwn.Api.Repositories;

public interface IDonationRepository
{
    Task<List<Donation>> GetAllAsync();
    Task<List<Donation>> GetAvailableAsync();
    Task<Donation?> GetByIdAsync(int donationId);
    Task AddAsync(Donation donation);
}

public class DonationRepository : IDonationRepository
{
    private readonly ApplicationDbContext _db;

    public DonationRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public Task<List<Donation>> GetAllAsync()
    {
        return _db.Donations
            .Include(d => d.Food)
            .Include(d => d.Clothes)
            .Include(d => d.Medicine)
            .Include(d => d.MedicalSupplies)
            .ToListAsync();
    }

    public Task<List<Donation>> GetAvailableAsync()
    {
        return _db.Donations
            .Where(d => d.Status == "Available")
            .Include(d => d.Food)
            .Include(d => d.Clothes)
            .Include(d => d.Medicine)
            .Include(d => d.MedicalSupplies)
            .ToListAsync();
    }

    public Task<Donation?> GetByIdAsync(int donationId)
    {
        return _db.Donations
            .Include(d => d.Food)
            .Include(d => d.Clothes)
            .Include(d => d.Medicine)
            .Include(d => d.MedicalSupplies)
            .FirstOrDefaultAsync(d => d.DonationId == donationId);
    }

    public Task AddAsync(Donation donation)
    {
        _db.Donations.Add(donation);
        return Task.CompletedTask;
    }
}
