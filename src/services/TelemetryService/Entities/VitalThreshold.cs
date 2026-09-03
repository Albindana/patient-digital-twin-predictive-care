namespace TelemetryService.Entities;

public class VitalThreshold
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid PatientId { get; set; }
    public int MaxHeartRate { get; set; } = 120;
    public int MinHeartRate { get; set; } = 50;
    public double MinOxygenLevel { get; set; } = 90.0;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}