using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace TelemetryService.BackgroundWorkers;

public class IoTSimulatorWorker : BackgroundService
{
    private readonly ILogger<IoTSimulatorWorker> _logger;
    private readonly HttpClient _httpClient;
    private readonly Random _random = new();

    // A dummy patient ID for testing simulation
    private static readonly Guid TestPatientId = Guid.NewGuid();

    public IoTSimulatorWorker(ILogger<IoTSimulatorWorker> logger, HttpClient httpClient)
    {
        _logger = logger;
        _httpClient = httpClient;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("IoT Simulator Worker started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            // Simulate realistic or occasionally anomalous vitals
            int heartRate = _random.Next(60, 110); 
            double oxygen = Math.Round(_random.Next(92, 100) + _random.NextDouble(), 1);

            // Occasionally inject an anomaly to test real-time alerts
            if (_random.Next(1, 20) == 1) 
            {
                heartRate = 145; // Spiked heart rate!
                oxygen = 88.5;   // Low oxygen!
                _logger.LogWarning("Simulating critical health anomaly for patient {PatientId}!", TestPatientId);
            }

            var telemetryPayload = new
            {
                PatientId = TestPatientId,
                HeartRate = heartRate,
                OxygenLevel = oxygen,
                Timestamp = DateTime.UtcNow
            };

            try
            {
                // Send payload to our own Telemetry Ingestion endpoint
                var response = await _httpClient.PostAsJsonAsync("http://localhost:8080/api/telemetry", telemetryPayload, stoppingToken);
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Failed to send telemetry data. Status code: {StatusCode}", response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while sending simulated telemetry data.");
            }

            // Wait 4 seconds before next reading
            await Task.Delay(TimeSpan.FromSeconds(4), stoppingToken);
        }
    }
}