using System.ComponentModel.DataAnnotations;

namespace YadElAwn.Api.Dtos;

public class CreateUserRequest
{
    [Required]
    public string FName { get; set; } = string.Empty;

    [Required]
    public string LName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;

    public string? Phone { get; set; }
    public string? Address { get; set; }
    public bool IsVerified { get; set; } = false;
    public string? UserType { get; set; }
}

public class RegisterDonorRequest
{
    [Required]
    public string FName { get; set; } = string.Empty;

    [Required]
    public string LName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;

    public string? Phone { get; set; }
    public string? Address { get; set; }
}

public class RegisterCharityRequest
{
    [Required]
    public string FName { get; set; } = string.Empty;

    [Required]
    public string LName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;

    public string? Phone { get; set; }
    public string? Address { get; set; }

    public int? Capacity { get; set; }
    public string? LicenseNumber { get; set; }
    public string? CoverageArea { get; set; }
    public string? Needs { get; set; }
    public int? LocationId { get; set; }
}

public class RegisterBeneficiaryRequest
{
    [Required]
    public string FName { get; set; } = string.Empty;

    [Required]
    public string LName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;

    public string? Phone { get; set; }
    public string? Address { get; set; }
    public int? LocationId { get; set; }
}

public class RegisterAdminRequest
{
    [Required]
    public string FName { get; set; } = string.Empty;

    [Required]
    public string LName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;

    public string? Phone { get; set; }
    public string? Address { get; set; }
}

public class CreateLocationRequest
{
    [Required]
    public string CityArea { get; set; } = string.Empty;
    public string? GpsCoordinates { get; set; }
}

public class CreateDonationRequest
{
    [Required]
    public int DonorId { get; set; }

    public int? LocationId { get; set; }
    public string? Status { get; set; }
    public string? Image { get; set; }

    public FoodDetails? Food { get; set; }
    public ClothesDetails? Clothes { get; set; }
    public MedicineDetails? Medicine { get; set; }
    public MedicalSuppliesDetails? MedicalSupplies { get; set; }
}

public class FoodDetails
{
    public string? ProductName { get; set; }
    public string? FoodType { get; set; }
    public string? Category { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public string? Quantity { get; set; }
    public string? StorageCondition { get; set; }
    public string? ShelfLife { get; set; }
}

public class ClothesDetails
{
    public string? Season { get; set; }
    public string? Gender { get; set; }
    public string? Size { get; set; }
    public string? Condition { get; set; }
    public string? CleaningStatus { get; set; }
}

public class MedicineDetails
{
    public string? MedicineName { get; set; }
    public string? MedicineType { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public string? Quantity { get; set; }
    public string? SafetyConditions { get; set; }
}

public class MedicalSuppliesDetails
{
    public string? SupplyName { get; set; }
    public string? Condition { get; set; }
    public string? Quantity { get; set; }
}

public class UpdateDonationStatusRequest
{
    [Required]
    public string Status { get; set; } = string.Empty;
}

public class CreateMessageRequest
{
    [Required]
    public int SenderId { get; set; }

    [Required]
    public int ReceiverId { get; set; }

    [Required]
    public string Content { get; set; } = string.Empty;
}

public class CreateNotificationRequest
{
    [Required]
    public int UserId { get; set; }

    [Required]
    public string Content { get; set; } = string.Empty;
}

public class CreateMatchRequest
{
    [Required]
    public int DonationId { get; set; }

    [Required]
    public int CharityId { get; set; }

    [Required]
    public int BeneficiaryId { get; set; }

    public decimal? Distance { get; set; }
    public string? UrgencyLevel { get; set; }
}

public class LoginRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}

public class AuthResponse
{
    public int UserId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string? UserType { get; set; }
    public string Token { get; set; } = string.Empty;
}
