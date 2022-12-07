using Microsoft.AspNetCore.SignalR;

namespace ChantingApp.Api.Hubs;

public class ChantStreamHub : Hub
{
    private readonly ILogger<ChantStreamHub> _logger;

    public ChantStreamHub(ILogger<ChantStreamHub> logger)
    {
        _logger = logger;
    }

    public async Task StopStream(string name)
    {
        await Clients.All.SendAsync("StreamEnded", name);
    }

    public override Task OnConnectedAsync()
    {
        _logger.LogInformation("Client connected to hub");
        return base.OnConnectedAsync();
    }
}