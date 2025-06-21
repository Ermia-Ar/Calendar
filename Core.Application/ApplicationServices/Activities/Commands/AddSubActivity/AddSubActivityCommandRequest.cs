using Core.Domain.Enum;
using MediatR;

namespace Core.Application.ApplicationServices.Activities.Commands.AddSubActivity
{
    public sealed record AddSubActivityCommandRequest(
         string ActivityId,
         string? Description,
         DateTime StartDate,
         int DurationInMinute,
         int NotificationBeforeInMinute,
         string[] MemberIds
        ) : IRequest;
}
