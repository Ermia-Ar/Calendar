using Core.Domain.Enum;
using SharedKernel.Helper;

namespace Core.Application.ApplicationServices.Projects.Queries.GetActivities;

public record class GetActivityOfProjectQueryResponse(
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
    TimeSpan? NotificationBefore,
    ActivityCategory Category,
    bool IsCompleted,
    bool IsEdited
    ) : IResponse;
