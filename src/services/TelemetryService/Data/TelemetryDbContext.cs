using Microsoft.EntityFrameworkCore;
using TelemetryService.Entities;

namespace TelemetryService.Data;

public class TelemetryDbContext : DbContext
{
    public TelemetryDbContext(DbContextOptions<TelemetryDbContext> options) : base(options) { }

    public DbSet<VitalThreshold> VitalThresholds => Set<VitalThreshold>();
    public DbSet<HealthAnomaly> HealthAnomalies => Set<HealthAnomaly>();
    public DbSet<PatientDevice> PatientDevices => Set<PatientDevice>();
    public DbSet<TelemetryAlertConfig> TelemetryAlertConfigs => Set<TelemetryAlertConfig>();
    public DbSet<AnomalyAuditLog> AnomalyAuditLogs => Set<AnomalyAuditLog>();
}