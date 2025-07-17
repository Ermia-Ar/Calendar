using Core.Domain.Enum;
using SharedKernel.Helper;

namespace Core.Application.ApplicationServices.Activities.Queries.GetAll;

public sealed record GetAllActivitiesQueryResponse(
    long Id,
	long? ParentId,
    Guid OwnerId,
    string Title,
    string? Description,
    DateTime StartDate,
    DateTime CreatedDate,
    DateTime UpdateDate,
    TimeSpan? Duration,
    int NotificationBeforeInMinute,
	ActivityType Type,
    bool IsCompleted
) : IResponse;


