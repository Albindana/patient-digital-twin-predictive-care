namespace BillingService.Entities;

public class Invoice : BaseEntity
{
    public Guid SubscriptionId { get; set; }
    public decimal Amount { get; set; }
    public DateTime DueDate { get; set; }
    public string Status { get; set; } = "Unpaid"; // Unpaid, Paid, Overdue
}