using Core.Domain.Enum;
using Mapster;
using SharedKernel.Helper;

namespace Core.Application.ApplicationServices.Activities.Queries.GetAll;

public record class GetAllActivitiesQueryResponse
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
    TimeSpan? NotificationBefore,
    ActivityCategory Category,
    bool IsCompleted,
    bool IsEdited

) : IResponse;

public class GetAllActivitiesProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
          config.ForType<IResponse, GetAllActivitiesQueryResponse>();
    }
}
