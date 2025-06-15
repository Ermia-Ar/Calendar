using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Calendar.Api.Hubs;

[Authorize]
public class NotificationHub : Hub
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
}
