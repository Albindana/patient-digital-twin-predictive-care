namespace CareService.Entities;

public class Doctor : BaseEntity
{
    public Guid UserId { get; set; } // Reference to IdentityService User
    public string Specialization { get; set; } = string.Empty;
    public string LicenseNumber { get; set; } = string.Empty;
    public string HospitalAffinity { get; set; } = string.Empty;
}