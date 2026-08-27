using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using MongoDB.Driver;
using TelemetryService.Hubs;
using TelemetryService.Models;

[ApiController]
[Route("api/[controller]")]
public class TelemetryController : ControllerBase
{
    private readonly IMongoCollection<TelemetryLog> _telemetryCollection;
    private readonly IHubContext<TelemetryHub> _hubContext;

    public TelemetryController(IMongoDatabase mongoDatabase, IHubContext<TelemetryHub> hubContext)
    {
        _telemetryCollection = mongoDatabase.GetCollection<TelemetryLog>("RawTelemetryLogs");
        _hubContext = hubContext;
    }

    [HttpPost]
    public async Task<IActionResult> IngestTelemetry([FromBody] TelemetryLog log)
    {
        Console.WriteLine($"[API HIT] Received Vitals - HR: {log.HeartRate}, O2: {log.OxygenLevel}");
        try
        {
            // 1. Save high-frequency raw JSON log to MongoDB[cite: 1, 2]
            await _telemetryCollection.InsertOneAsync(log);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"MongoDb ingestion error: {ex.Message}");
            return StatusCode(500, "Db connection error");
        }
        // 2. Evaluate rules / thresholds for anomalies (e.g., Heart Rate > 120 or Oxygen < 90)
        if (log.HeartRate > 120 || log.OxygenLevel < 90.0)
        {
            // 3. Push real-time alert via SignalR to connected doctor/nurse dashboards[cite: 1, 2]
            await _hubContext.Clients.All.SendAsync("ReceiveCriticalAlert", new
            {
                Message = $"CRITICAL VITAL ANOMALY DETECTED!",
                log.PatientId,
                log.HeartRate,
                log.OxygenLevel,
                log.Timestamp
            });
        }

        // Always broadcast live telemetry update for live charts
        await _hubContext.Clients.Group(log.PatientId.ToString()).SendAsync("ReceiveTelemetryUpdate", log);

        return Ok(new { Status = "Logged and evaluated successfully" });
    }
}