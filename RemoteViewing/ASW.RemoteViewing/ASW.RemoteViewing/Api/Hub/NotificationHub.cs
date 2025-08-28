using Microsoft.AspNetCore.SignalR;

namespace ASW.RemoteViewing.Api.Hub;

public class NotificationHub : Microsoft.AspNetCore.SignalR.Hub
{
    public async Task NewProblems(string groupName)
    {
        await Clients.All.SendAsync("NewProblems");
    }
}