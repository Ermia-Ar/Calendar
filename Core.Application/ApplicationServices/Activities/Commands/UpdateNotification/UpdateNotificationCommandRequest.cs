using Amazon.Runtime.Internal.Auth;
using MediatR;

namespace Core.Application.ApplicationServices.Activities.Commands.UpdateNotification;


/// <summary>
/// 
/// </summary>
/// <param name="ActivityId">ایدی فعالیت</param>
/// <param name="NotificationBefore"></param>
public sealed record UpdateNotificationCommandRequest
(
	string ActivityId,
	TimeSpan? NotificationBefore
) : IRequest;
