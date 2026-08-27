using MongoDB.Driver;
using TelemetryService.BackgroundWorkers;
using TelemetryService.Hubs;

var builder = WebApplication.CreateBuilder(args);

// 1. Safe MongoDB Registration with Fallback
var mongoConnectionString = builder.Configuration.GetConnectionString("MongoConnection")
    ?? "mongodb://localhost:27017";

builder.Services.AddSingleton<IMongoClient>(sp => new MongoClient(mongoConnectionString));

builder.Services.AddScoped<IMongoDatabase>(sp =>
{
    var client = sp.GetRequiredService<IMongoClient>();
    var dbName = builder.Configuration["ConnectionStrings:DatabaseName"] ?? "TelemetryDb";
    return client.GetDatabase(dbName);
});

// 2. Enable CORS for SignalR & React Frontend
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:5173", "http://localhost:5050")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// SignalR & Controller Registration
builder.Services.AddSignalR();
builder.Services.AddControllers();

// Register IoT Simulator Background Worker & HttpClient
builder.Services.AddHttpClient();
builder.Services.AddHostedService<IoTSimulatorWorker>();

var app = builder.Build();

// Enable CORS Middleware
app.UseCors();

app.MapControllers();
app.MapHub<TelemetryHub>("/hubs/telemetry");

app.Run();