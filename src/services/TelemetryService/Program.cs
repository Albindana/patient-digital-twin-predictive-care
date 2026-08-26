using MongoDB.Driver;
using TelemetryService.BackgroundWorkers;
using TelemetryService.Hubs;

var builder = WebApplication.CreateBuilder(args);

// MongoDB Registration
builder.Services.AddSingleton<IMongoClient>(sp =>
    new MongoClient(builder.Configuration.GetConnectionString("MongoConnection")));

builder.Services.AddScoped<IMongoDatabase>(sp =>
{
    var client = sp.GetRequiredService<IMongoClient>();
    return client.GetDatabase(builder.Configuration["ConnectionStrings:DatabaseName"] ?? "TelemetryDb");
});

// SignalR Registration
builder.Services.AddSignalR();
builder.Services.AddControllers();

// Register IoT Simulator Background Worker & HttpClient
builder.Services.AddHttpClient();
builder.Services.AddHostedService<IoTSimulatorWorker>();

var app = builder.Build();

app.MapControllers();
app.MapHub<TelemetryHub>("/hubs/telemetry");

app.Run();