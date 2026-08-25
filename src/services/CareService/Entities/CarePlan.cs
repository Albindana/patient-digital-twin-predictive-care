namespace CareService.Entities;

public class CarePlan : BaseEntity
{
    public Guid PatientId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Status { get; set; } = "Active";
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}