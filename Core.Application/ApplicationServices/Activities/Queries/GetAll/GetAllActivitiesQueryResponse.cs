using Core.Domain.Enum;
using SharedKernel.Helper;

namespace Core.Application.ApplicationServices.Activities.Queries.GetAll;

public sealed record GetAllActivitiesQueryResponse
(
    string Id,
    string? ParentId,
    string ProjectId,
    string UserId,
    string Title,
    string? Description,
    DateTime StartDate,
    DateTime CreatedDate,
    DateTime UpdateDate,
    TimeSpan? Duration,
    int NotificationBeforeInMinute,
	ActivityCategory Category,
    bool IsCompleted,
    bool IsEdited
    

) : IResponse;


