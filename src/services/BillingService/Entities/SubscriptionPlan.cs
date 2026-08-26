namespace BillingService.Entities;

public class SubscriptionPlan : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public decimal MonthlyPrice { get; set; }
    public int MaxDevices { get; set; }
    public string Description { get; set; } = string.Empty;
}