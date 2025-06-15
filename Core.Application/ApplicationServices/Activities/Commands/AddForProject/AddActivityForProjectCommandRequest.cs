using Core.Domain.Enum;
using MediatR;

namespace Core.Application.Features.Activities.Commands;

public sealed record class AddActivityForProjectCommandRequest(
     string ProjectId ,
     string Title ,
     string? Description ,
     DateTime StartDate ,
     int DurationInMinute ,
     int NotificationBeforeInMinute ,
     ActivityCategory Category 
    ) : IRequest;
