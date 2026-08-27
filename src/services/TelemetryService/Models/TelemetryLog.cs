using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TelemetryService.Models;

public class TelemetryLog
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid PatientId { get; set; }
    public int HeartRate { get; set; }
    public double OxygenLevel { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}