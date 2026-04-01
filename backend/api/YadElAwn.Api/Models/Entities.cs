using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YadElAwn.Api.Models;

public class Location
{
    [Key]
    [Column("LocationID")]
    public int LocationId { get; set; }

    [Column("City_Area")]
    [MaxLength(100)]
    public string CityArea { get; set; } = string.Empty;

    [Column("GPS_Coordinates")]
    [MaxLength(100)]
    public string? GpsCoordinates { get; set; }
}

public class User
{
    [Key]
    [Column("UserID")]
    public int UserId { get; set; }

    [Column("FName")]
    [MaxLength(50)]
    public string FName { get; set; } = string.Empty;

    [Column("LName")]
    [MaxLength(50)]
    public string LName { get; set; } = string.Empty;

    [Column("Email")]
    [MaxLength(100)]
    public string Email { get; set; } = string.Empty;

    [Column("Password")]
    [MaxLength(255)]
    public string Password { get; set; } = string.Empty;

    [Column("Phone")]
    [MaxLength(20)]
    public string? Phone { get; set; }

    [Column("Address")]
    public string? Address { get; set; }

    [Column("IsVerified")]
    public bool IsVerified { get; set; }

    [Column("UserType")]
    [MaxLength(20)]
    public string? UserType { get; set; }
}

public class Donor
{
    [Key]
    [Column("DonorID")]
    public int DonorId { get; set; }

    [Column("Donation_Count")]
    public int DonationCount { get; set; }

    public User? User { get; set; }
}

public class Charity
{
    [Key]
    [Column("CharityID")]
    public int CharityId { get; set; }

    [Column("Capacity")]
    public int? Capacity { get; set; }

    [Column("License_Number")]
    [MaxLength(50)]
    public string? LicenseNumber { get; set; }

    [Column("CoverageArea")]
    [MaxLength(100)]
    public string? CoverageArea { get; set; }

    [Column("Needs")]
    public string? Needs { get; set; }

    [Column("LocationID")]
    public int? LocationId { get; set; }

    public User? User { get; set; }
    public Location? Location { get; set; }
}

public class Beneficiary
{
    [Key]
    [Column("BeneficiaryID")]
    public int BeneficiaryId { get; set; }

    [Column("LocationID")]
    public int? LocationId { get; set; }

    public User? User { get; set; }
    public Location? Location { get; set; }
}

public class Admin
{
    [Key]
    [Column("AdminID")]
    public int AdminId { get; set; }

    public User? User { get; set; }
}

public class Donation
{
    [Key]
    [Column("DonationID")]
    public int DonationId { get; set; }

    [Column("Status")]
    [MaxLength(50)]
    public string? Status { get; set; }

    [Column("Image")]
    [MaxLength(255)]
    public string? Image { get; set; }

    [Column("DonorID")]
    public int DonorId { get; set; }

    [Column("LocationID")]
    public int? LocationId { get; set; }

    public Donor? Donor { get; set; }
    public Location? Location { get; set; }
    public Food? Food { get; set; }
    public Clothes? Clothes { get; set; }
    public Medicine? Medicine { get; set; }
    public MedicalSupplies? MedicalSupplies { get; set; }
}

public class Food
{
    [Key]
    [Column("DonationID")]
    public int DonationId { get; set; }

    [Column("Product_Name")]
    [MaxLength(100)]
    public string? ProductName { get; set; }

    [Column("Food_Type")]
    [MaxLength(50)]
    public string? FoodType { get; set; }

    [Column("Category")]
    [MaxLength(50)]
    public string? Category { get; set; }

    [Column("Expiry_Date")]
    public DateTime? ExpiryDate { get; set; }

    [Column("Quantity")]
    [MaxLength(50)]
    public string? Quantity { get; set; }

    [Column("Storage_Condition")]
    [MaxLength(100)]
    public string? StorageCondition { get; set; }

    [Column("Shelf_Life")]
    [MaxLength(50)]
    public string? ShelfLife { get; set; }

    public Donation? Donation { get; set; }
}

public class Clothes
{
    [Key]
    [Column("DonationID")]
    public int DonationId { get; set; }

    [Column("Season")]
    [MaxLength(50)]
    public string? Season { get; set; }

    [Column("Gender")]
    [MaxLength(50)]
    public string? Gender { get; set; }

    [Column("Size")]
    [MaxLength(20)]
    public string? Size { get; set; }

    [Column("Condition")]
    [MaxLength(50)]
    public string? Condition { get; set; }

    [Column("Cleaning_Status")]
    [MaxLength(100)]
    public string? CleaningStatus { get; set; }

    public Donation? Donation { get; set; }
}

public class Medicine
{
    [Key]
    [Column("DonationID")]
    public int DonationId { get; set; }

    [Column("Medicine_Name")]
    [MaxLength(100)]
    public string? MedicineName { get; set; }

    [Column("Medicine_Type")]
    [MaxLength(100)]
    public string? MedicineType { get; set; }

    [Column("Expiry_Date")]
    public DateTime? ExpiryDate { get; set; }

    [Column("Quantity")]
    [MaxLength(50)]
    public string? Quantity { get; set; }

    [Column("Safety_Conditions")]
    public string? SafetyConditions { get; set; }

    public Donation? Donation { get; set; }
}

public class MedicalSupplies
{
    [Key]
    [Column("DonationID")]
    public int DonationId { get; set; }

    [Column("Supply_Name")]
    [MaxLength(100)]
    public string? SupplyName { get; set; }

    [Column("Condition")]
    [MaxLength(100)]
    public string? Condition { get; set; }

    [Column("Quantity")]
    [MaxLength(50)]
    public string? Quantity { get; set; }

    public Donation? Donation { get; set; }
}

public class StatusHistory
{
    [Key]
    [Column("HistoryID")]
    public int HistoryId { get; set; }

    [Column("DonationID")]
    public int DonationId { get; set; }

    [Column("Old_Status")]
    [MaxLength(50)]
    public string? OldStatus { get; set; }

    [Column("New_Status")]
    [MaxLength(50)]
    public string? NewStatus { get; set; }

    [Column("Change_Date")]
    public DateTime ChangeDate { get; set; }

    public Donation? Donation { get; set; }
}

public class Match
{
    [Key]
    [Column("MatchID")]
    public int MatchId { get; set; }

    [Column("DonationID")]
    public int DonationId { get; set; }

    [Column("CharityID")]
    public int CharityId { get; set; }

    [Column("BeneficiaryID")]
    public int BeneficiaryId { get; set; }

    [Column("Distance")]
    public decimal? Distance { get; set; }

    [Column("Urgency_Level")]
    [MaxLength(50)]
    public string? UrgencyLevel { get; set; }

    [Column("Match_Date")]
    public DateTime MatchDate { get; set; }

    public Donation? Donation { get; set; }
    public Charity? Charity { get; set; }
    public Beneficiary? Beneficiary { get; set; }
}

public class Message
{
    [Key]
    [Column("MessageID")]
    public int MessageId { get; set; }

    [Column("SenderID")]
    public int SenderId { get; set; }

    [Column("ReceiverID")]
    public int ReceiverId { get; set; }

    [Column("Content")]
    public string? Content { get; set; }

    [Column("Sent_Date_Time")]
    public DateTime SentDateTime { get; set; }

    [Column("Is_Read")]
    public bool IsRead { get; set; }

    public User? Sender { get; set; }
    public User? Receiver { get; set; }
}

public class Notification
{
    [Key]
    [Column("NotifID")]
    public int NotifId { get; set; }

    [Column("UserID")]
    public int UserId { get; set; }

    [Column("Content")]
    public string? Content { get; set; }

    [Column("Timestamp")]
    public DateTime Timestamp { get; set; }

    [Column("Is_Read")]
    public bool IsRead { get; set; }

    public User? User { get; set; }
}

public class AuditLog
{
    [Key]
    [Column("LogID")]
    public int LogId { get; set; }

    [Column("AdminID")]
    public int AdminId { get; set; }

    [Column("Action_Taken")]
    public string? ActionTaken { get; set; }

    [Column("Action_Date")]
    public DateTime ActionDate { get; set; }

    public Admin? Admin { get; set; }
}

public class Evaluate
{
    [Column("CharityID")]
    public int CharityId { get; set; }

    [Column("DonorID")]
    public int DonorId { get; set; }

    [Column("Score")]
    public int Score { get; set; }

    [Column("Comment")]
    public string? Comment { get; set; }

    public Charity? Charity { get; set; }
    public Donor? Donor { get; set; }
}
