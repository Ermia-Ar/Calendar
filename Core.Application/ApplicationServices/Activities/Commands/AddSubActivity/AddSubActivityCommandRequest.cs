using Core.Domain.Enum;
using MediatR;

namespace Core.Application.ApplicationServices.Activities.Commands.AddSubActivity
{
    public record class AddSubActivityCommandRequest(
         string ActivityId,
         string Title,
         string? Description,
         DateTime StartDate,
         int DurationInMinute,
         int NotificationBeforeInMinute,
         ActivityCategory Category 
        ) : IRequest;
}
