namespace TelemetryService.Entities;

public class PatientDevice
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid PatientId { get; set; }
    public string DeviceSerialNumber { get; set; } = string.Empty;
    public string DeviceModel { get; set; } = "IoT-Monitor-V1";
    public bool IsActive { get; set; } = true;
    public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;
}