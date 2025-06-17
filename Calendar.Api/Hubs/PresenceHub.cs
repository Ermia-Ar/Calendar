using Microsoft.AspNetCore.SignalR;

namespace Calendar.Api.Hubs;

public class PresenceHub : Hub
{
    public override Task OnConnectedAsync()
    {
        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        return base.OnDisconnectedAsync(exception);
    }
    public async Task JoinGroup(string groupName
        , CancellationToken token = default)
    {
        var connectionId = Context.ConnectionId;
        await Groups.AddToGroupAsync(connectionId, groupName, token);
    }

    public async Task LeftGroup(string groupName
        , CancellationToken token = default)
    {
        var connectionId = Context.ConnectionId;
        await Groups.AddToGroupAsync(connectionId, groupName, token);
    }

    public async Task SendProjectRequests(List<string> receiverIds, string projectId, string message)
    {
        foreach (var receiverId in receiverIds)
        {
            await Clients.User(receiverId).SendAsync("ReceiveRequest", projectId, message);
        }
    }
}
