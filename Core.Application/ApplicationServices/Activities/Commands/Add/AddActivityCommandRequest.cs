using Core.Domain.Enum;
using MediatR;

namespace Core.Application.ApplicationServices.Activities.Commands.Add;

public sealed record AddActivityCommandRequest(
     string Title ,
     string? Description ,
     DateTime StartDate ,
     int? DurationInMinute ,
     int? NotificationBeforeInMinute ,
     ActivityCategory Category ,
     string[] Members ,
     string? Message
) : IRequest;
