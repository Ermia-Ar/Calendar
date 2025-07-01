using Core.Application.ApplicationServices.Activities.Queries.GetAll;
using Core.Application.ApplicationServices.Requests.Queries.GetAll;
using Core.Domain.Enum;
using MediatR;

namespace Core.Application.ApplicationServices.Projects.Commands.AddActivity;

public sealed record class AddActivityForProjectCommandRequest(
     string ProjectId,
     string Title,
     string? Description,
     DateTime StartDate,
     TimeSpan? Duration,
     TimeSpan? NotificationBefore,
     ActivityCategory Category,
     string[] MemberIds
    ) : IRequest<Dictionary<string, GetAllRequestQueryResponse>>;
