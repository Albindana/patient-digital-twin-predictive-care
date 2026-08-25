namespace CareService.Entities;

public class Patient : BaseEntity
{
    public Guid UserId { get; set; } // Reference to IdentityService User
    public DateTime DateOfBirth { get; set; }
    public string BloodType { get; set; } = string.Empty;
    public string EmergencyContact { get; set; } = string.Empty;
    public string InsuranceNumber { get; set; } = string.Empty;
}