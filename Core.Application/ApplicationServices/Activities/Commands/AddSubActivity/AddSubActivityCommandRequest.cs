using Core.Application.ApplicationServices.Requests.Queries.GetAll;
using MediatR;

namespace Core.Application.ApplicationServices.Activities.Commands.AddSubActivity;

public sealed record AddSubActivityCommandRequest(
	string ActivityId,
	string? Description,
	DateTime StartDate,
	TimeSpan? Duration,
	TimeSpan? NotificationBefore,
	string[] MemberIds

) : IRequest<Dictionary<string, GetAllRequestQueryResponse>>;
