using Core.Application.ApplicationServices.Requests.Queries.GetAll;
using Core.Domain.Entities.Requests;
using Core.Domain.Enum;
using MediatR;

namespace Core.Application.ApplicationServices.Activities.Commands.Add;

public sealed record AddActivityCommandRequest(
     string Title ,
     string? Description ,
     DateTime StartDate ,
     TimeSpan? Duration ,
     TimeSpan? NotificationBefore ,
     ActivityCategory Category ,
     string[] MemberIds ,
     string? Message
) : IRequest<Dictionary<string, GetAllRequestQueryResponse>>;
