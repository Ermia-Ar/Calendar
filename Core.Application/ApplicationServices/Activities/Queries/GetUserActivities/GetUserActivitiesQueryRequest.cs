using Core.Domain.Enum;
using MediatR;

namespace Core.Application.ApplicationServices.Activities.Queries.GetUserActivities;

public record class GetUserActivitiesQueryRequest
(
    DateTime? StartDate,
    bool UserIsOwner,
    bool IsCompleted,
    bool IsHistory,
    ActivityCategory? Category
)
    : IRequest<List<GetUserActivitiesQueryResponse>>;
