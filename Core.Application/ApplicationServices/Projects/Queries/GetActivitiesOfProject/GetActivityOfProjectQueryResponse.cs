using Core.Domain.Enum;
using Mapster;
using SharedKernel.Helper;

namespace Core.Application.ApplicationServices.Projects.Queries.GetActivitiesOfProject;

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

public class GetActivityOfProjectProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.ForType<IResponse, GetActivityOfProjectQueryResponse>();
    }
}
