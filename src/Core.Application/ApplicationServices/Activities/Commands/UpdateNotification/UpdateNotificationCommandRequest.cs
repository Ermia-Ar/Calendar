using MediatR;

namespace Core.Application.ApplicationServices.Activities.Commands.UpdateNotification;


/// <summary>
/// 
/// </summary>
/// <param name="ActivityId">ایدی فعالیت</param>
/// <param name="NotificationBefore"></param>

public sealed record UpdateNotificationCommandRequest
(
	long ActivityId,
	TimeSpan? NotificationBefore
) : IRequest
{
	public static UpdateNotificationCommandRequest Create(long id, UpdateNotificationDto model) 
		=> new UpdateNotificationCommandRequest(id, model.NotificationBefore);
}

/// <summary>
/// 
/// </summary>
/// <param name="NotificationBefore"> چه مدت مانده به فعالیت اعلان داده شود</param>
public sealed record UpdateNotificationDto (
	TimeSpan? NotificationBefore
	);

