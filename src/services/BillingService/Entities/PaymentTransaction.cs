namespace BillingService.Entities;

public class PaymentTransaction : BaseEntity
{
    public Guid InvoiceId { get; set; }
    public string ProviderTransactionId { get; set; } = string.Empty;
    public string PaymentMethod { get; set; } = "CreditCard"; // CreditCard, Stripe, PayPal
    public decimal Amount { get; set; }
    public string Status { get; set; } = "Pending"; // Success, Failed, Pending
}