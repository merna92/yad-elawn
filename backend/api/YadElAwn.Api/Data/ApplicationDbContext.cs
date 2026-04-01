using Microsoft.EntityFrameworkCore;
using YadElAwn.Api.Models;

namespace YadElAwn.Api.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Location> Locations => Set<Location>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Donor> Donors => Set<Donor>();
    public DbSet<Charity> Charities => Set<Charity>();
    public DbSet<Beneficiary> Beneficiaries => Set<Beneficiary>();
    public DbSet<Admin> Admins => Set<Admin>();
    public DbSet<Donation> Donations => Set<Donation>();
    public DbSet<Food> Foods => Set<Food>();
    public DbSet<Clothes> Clothes => Set<Clothes>();
    public DbSet<Medicine> Medicines => Set<Medicine>();
    public DbSet<MedicalSupplies> MedicalSupplies => Set<MedicalSupplies>();
    public DbSet<StatusHistory> StatusHistories => Set<StatusHistory>();
    public DbSet<Match> Matches => Set<Match>();
    public DbSet<Message> Messages => Set<Message>();
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();
    public DbSet<Evaluate> Evaluates => Set<Evaluate>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Location>().ToTable("Location");
        modelBuilder.Entity<User>().ToTable("User");
        modelBuilder.Entity<Donor>().ToTable("Donor");
        modelBuilder.Entity<Charity>().ToTable("Charity");
        modelBuilder.Entity<Beneficiary>().ToTable("Beneficiary");
        modelBuilder.Entity<Admin>().ToTable("Admin");
        modelBuilder.Entity<Donation>().ToTable("Donation");
        modelBuilder.Entity<Food>().ToTable("Food");
        modelBuilder.Entity<Clothes>().ToTable("Clothes");
        modelBuilder.Entity<Medicine>().ToTable("Medicine");
        modelBuilder.Entity<MedicalSupplies>().ToTable("Medical_Supplies");
        modelBuilder.Entity<StatusHistory>().ToTable("Status_History");
        modelBuilder.Entity<Match>().ToTable("Matches");
        modelBuilder.Entity<Message>().ToTable("Message");
        modelBuilder.Entity<Notification>().ToTable("Notification");
        modelBuilder.Entity<AuditLog>().ToTable("Audit_Log");
        modelBuilder.Entity<Evaluate>().ToTable("Evaluates");

        modelBuilder.Entity<Donor>()
            .HasOne(d => d.User)
            .WithOne()
            .HasForeignKey<Donor>(d => d.DonorId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Charity>()
            .HasOne(c => c.User)
            .WithOne()
            .HasForeignKey<Charity>(c => c.CharityId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Beneficiary>()
            .HasOne(b => b.User)
            .WithOne()
            .HasForeignKey<Beneficiary>(b => b.BeneficiaryId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Admin>()
            .HasOne(a => a.User)
            .WithOne()
            .HasForeignKey<Admin>(a => a.AdminId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Donation>()
            .HasOne(d => d.Donor)
            .WithMany()
            .HasForeignKey(d => d.DonorId);

        modelBuilder.Entity<Donation>()
            .HasOne(d => d.Location)
            .WithMany()
            .HasForeignKey(d => d.LocationId);

        modelBuilder.Entity<Food>()
            .HasOne(f => f.Donation)
            .WithOne(d => d.Food)
            .HasForeignKey<Food>(f => f.DonationId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Clothes>()
            .HasOne(c => c.Donation)
            .WithOne(d => d.Clothes)
            .HasForeignKey<Clothes>(c => c.DonationId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Medicine>()
            .HasOne(m => m.Donation)
            .WithOne(d => d.Medicine)
            .HasForeignKey<Medicine>(m => m.DonationId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<MedicalSupplies>()
            .HasOne(ms => ms.Donation)
            .WithOne(d => d.MedicalSupplies)
            .HasForeignKey<MedicalSupplies>(ms => ms.DonationId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<StatusHistory>()
            .HasOne(sh => sh.Donation)
            .WithMany()
            .HasForeignKey(sh => sh.DonationId);

        modelBuilder.Entity<Match>()
            .HasOne(m => m.Donation)
            .WithMany()
            .HasForeignKey(m => m.DonationId);

        modelBuilder.Entity<Match>()
            .HasOne(m => m.Charity)
            .WithMany()
            .HasForeignKey(m => m.CharityId);

        modelBuilder.Entity<Match>()
            .HasOne(m => m.Beneficiary)
            .WithMany()
            .HasForeignKey(m => m.BeneficiaryId);

        modelBuilder.Entity<Message>()
            .HasOne(m => m.Sender)
            .WithMany()
            .HasForeignKey(m => m.SenderId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Message>()
            .HasOne(m => m.Receiver)
            .WithMany()
            .HasForeignKey(m => m.ReceiverId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Notification>()
            .HasOne(n => n.User)
            .WithMany()
            .HasForeignKey(n => n.UserId);

        modelBuilder.Entity<AuditLog>()
            .HasOne(a => a.Admin)
            .WithMany()
            .HasForeignKey(a => a.AdminId);

        modelBuilder.Entity<Evaluate>()
            .HasKey(e => new { e.CharityId, e.DonorId });

        modelBuilder.Entity<Evaluate>()
            .HasOne(e => e.Charity)
            .WithMany()
            .HasForeignKey(e => e.CharityId);

        modelBuilder.Entity<Evaluate>()
            .HasOne(e => e.Donor)
            .WithMany()
            .HasForeignKey(e => e.DonorId);
    }
}
