using MediatR;

namespace Core.Application.ApplicationServices.Activities.Commands.SetNotification;

public sealed record SetNotificationCommandRequest(
    long ActivityId,
    TimeSpan NotificationBefore
    ) : IRequest;