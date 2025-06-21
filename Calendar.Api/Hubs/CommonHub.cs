using Core.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Calendar.Api.Hubs;

[Authorize]
public class CommonHub(ICurrentUserServices userServices, IUnitOfWork unitOfWork) : Hub
{
	private readonly ICurrentUserServices _currentUserServices = userServices;
	private readonly IUnitOfWork _unitOfWork = unitOfWork;

	public override async Task OnConnectedAsync()
	{
		await base.OnConnectedAsync();
		await Join();
		Console.WriteLine("User Connected");
	}

	public override async Task OnDisconnectedAsync(Exception? exception)
	{
        await base.OnDisconnectedAsync(exception);
		Console.WriteLine("User Disconnected");
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

		var projectIds = await _unitOfWork
			.Requests.FindProjectIds(userId, token);
		var activityIds = await _unitOfWork
			.Requests.FindActivityIds(userId, token);

		foreach (var activityId in activityIds)
		{
			await Groups.AddToGroupAsync(Context.ConnectionId, activityId, token);
		}

		foreach (var projectId in projectIds)
		{
			await Groups.AddToGroupAsync(Context.ConnectionId, projectId, token);
		}
	}

	public async Task LeftGroup(string groupName
		, CancellationToken token = default)
	{
		var connectionId = Context.ConnectionId;
		await Groups.AddToGroupAsync(connectionId, groupName, token);
	}
}
