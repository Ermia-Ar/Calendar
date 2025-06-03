using Core.Application.DTOs.ActivityDTOs;
using Core.Domain.Enum;
using MediatR;

namespace Core.Application.ApplicationServices.Activities.Commands.UpdateActivity
{
    public record class UpdateActivityCommand(
        string Id,
        string Title,
        string? Description,
        DateTime StartDate,
        int? DurationInMinute,
        int? NotificationBeforeInMinute,
        ActivityCategory Category,
        bool isCompleted
            ): IRequest;
}
