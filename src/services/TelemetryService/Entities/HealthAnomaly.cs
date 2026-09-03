namespace TelemetryService.Entities;

public class HealthAnomaly
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid PatientId { get; set; }
    public string AnomalyType { get; set; } = string.Empty; // e.g., TACHYCARDIA, HYPOXIA
    public string Severity { get; set; } = "High";
    public int HeartRate { get; set; }
    public double OxygenLevel { get; set; }
    public DateTime DetectedAt { get; set; } = DateTime.UtcNow;
    public bool IsResolved { get; set; } = false;
}