namespace TelemetryService.Entities;

public class TelemetryAlertConfig
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid PatientId { get; set; }
    public string EmergencyEmail { get; set; } = string.Empty;
    public bool EnablePushNotifications { get; set; } = true;
    public bool EnableSmsAlerts { get; set; } = false;
}