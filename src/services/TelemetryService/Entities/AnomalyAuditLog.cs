namespace TelemetryService.Entities;

public class AnomalyAuditLog
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid AnomalyId { get; set; }
    public string ActionTaken { get; set; } = string.Empty; // e.g., "Doctor Notified", "Dismissed"
    public string PerformedBy { get; set; } = "System";
    public DateTime ActionTimestamp { get; set; } = DateTime.UtcNow;
}