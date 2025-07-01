using Core.Application.Common.Event;
using Infrastructure.ExternalServices.SignalR;

namespace Calendar.Api.Hubs;

[Authorize]
public class CommonHub(IGroupServices groupServices, IOnlineClientManager onlineClientManager) : Hub
{
	private readonly IGroupServices _groupServices = groupServices;
	private readonly IOnlineClientManager _onlineClientManager = onlineClientManager;

    public override async Task OnConnectedAsync()
	{
		await base.OnConnectedAsync();
		await Join();

		_onlineClientManager.ClientDisconnected += _onlineClientManager_ClientDisconnected;
		_onlineClientManager.ClientConnected += _onlineClientManager_ClientConnected;
		_onlineClientManager.UserDisconnected += _onlineClientManager_UserDisconnected;
		_onlineClientManager.UserConnected += _onlineClientManager_UserConnected;

		_onlineClientManager.Add(new OnlineClient(Context.ConnectionId, Context.UserIdentifier));
	}

	private void _onlineClientManager_UserConnected(object? sender, OnlineUserEventArgs e)
	{
	}

	private void _onlineClientManager_UserDisconnected(object? sender, OnlineUserEventArgs e)
	{
		_onlineClientManager.Remove(e.Client);
	}

	private void _onlineClientManager_ClientConnected(object? sender, OnlineClientEventArgs e)
	{
	}

	public void _onlineClientManager_ClientDisconnected(object? sender, OnlineClientEventArgs e)
	{
		_onlineClientManager.Remove(e.Client);
	}

	public override async Task OnDisconnectedAsync(Exception? exception)
	{
		await base.OnDisconnectedAsync(exception);
		Console.WriteLine("User Disconnected");
		_onlineClientManager.Remove(Context.ConnectionId);
	}

	public async Task JoinGroup(string groupName
		, CancellationToken token = default)
	{
		var connectionId = Context.ConnectionId;
		await Groups.AddToGroupAsync(connectionId, groupName, token);
	}

	public async Task Join(CancellationToken token = default)
	{
		var userId = Context.UserIdentifier;

		await Groups.AddToGroupAsync(Context.ConnectionId, userId, cancellationToken :token);

		var groupNames = await _groupServices.GetGroups(userId, token);

		foreach (var groupName in groupNames)
		{
			await Groups.AddToGroupAsync(Context.ConnectionId, groupName, token);
		}
	}

	public async Task LeftGroup(string groupName
		, CancellationToken token = default)
	{
		var connectionId = Context.ConnectionId;
		await Groups.AddToGroupAsync(connectionId, groupName, token);
	}

}
