namespace BillingService.Entities;

public class PatientSubscription : BaseEntity
{
    public Guid PatientId { get; set; }
    public Guid PlanId { get; set; }
    public DateTime StartDate { get; set; } = DateTime.UtcNow;
    public DateTime? EndDate { get; set; }
    public string Status { get; set; } = "Active"; // Active, Cancelled, Paused
}