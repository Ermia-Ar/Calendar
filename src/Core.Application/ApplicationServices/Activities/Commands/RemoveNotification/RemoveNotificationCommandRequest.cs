using MediatR;

namespace Core.Application.ApplicationServices.Activities.Commands.RemoveNotification;

public sealed record RemoveNotificationCommandRequest(
    long ActivityId
) : IRequest;