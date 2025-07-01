using MediatR;

namespace Core.Application.ApplicationServices.Activities.Commands.UpdateNotification;

public sealed record UpdateNotificationCommandRequest
(
	string ActivityId,
	TimeSpan NotificationBefore
) : IRequest;
