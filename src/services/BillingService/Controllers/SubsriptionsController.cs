using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BillingService.Data;
using BillingService.Entities;

namespace BillingService.Controllers;

[ApiController]
[Route("api/billing/[controller]")]
public class SubscriptionsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public SubscriptionsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("plans")]
    public async Task<IActionResult> GetPlans()
    {
        var plans = await _context.SubscriptionPlans.ToListAsync();
        return Ok(plans);
    }

    [HttpPost("subscribe")]
    public async Task<IActionResult> CreateSubscription([FromBody] PatientSubscription subscription)
    {
        _context.PatientSubscriptions.Add(subscription);

        // Fetch plan to create the initial invoice
        var plan = await _context.SubscriptionPlans.FindAsync(subscription.PlanId);
        
        var invoice = new Invoice
        {
            SubscriptionId = subscription.Id,
            Amount = plan?.MonthlyPrice ?? 99.99m,
            DueDate = DateTime.UtcNow.AddDays(30),
            Status = "Unpaid",
            CreatedBy = "System"
        };
        _context.Invoices.Add(invoice);

        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetPlans), new { id = subscription.Id }, new { subscription, invoice });
    }
}