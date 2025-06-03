using Core.Application.DTOs.ActivityDTOs;
using Core.Domain.Enum;
using MediatR;

namespace Core.Application.Features.Activities.Commands
{
    public record class AddActivityForProjectCommandRequest(
         string ProjectId ,
         string Title ,
         string? Description ,
         DateTime StartDate ,
         int? DurationInMinute ,
         int? NotificationBeforeInMinute ,
         ActivityCategory Category 
        ) : IRequest;
}
