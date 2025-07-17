using Core.Domain.Enum;
using SharedKernel.Helper;

namespace Core.Application.ApplicationServices.Projects.Queries.Activities;

public sealed record GetProjectActivitiesQueryResponse(
    long Id,
    long? ParentId,
    long ProjectId,
    Guid OwnerId,
    string Title,
    string? Description,
    DateTime StartDate,
    DateTime CreatedDate,
    DateTime UpdateDate,
    TimeSpan? Duration,
    TimeSpan? NotificationBeforeInMinute,
    ActivityType Type,
    bool IsCompleted


) : IResponse;
