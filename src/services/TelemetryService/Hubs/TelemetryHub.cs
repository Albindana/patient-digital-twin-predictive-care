using Microsoft.AspNetCore.SignalR;

namespace TelemetryService.Hubs;

public class TelemetryHub : Hub
{
    // Clients can join specific patient rooms to watch their digital twin updates
    public async Task JoinPatientRoom(string patientId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, patientId);
    }

    public async Task LeavePatientRoom(string patientId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, patientId);
    }
}