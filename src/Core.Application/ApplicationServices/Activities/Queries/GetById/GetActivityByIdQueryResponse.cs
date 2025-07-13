using Core.Domain.Enum;
using Mapster;
using SharedKernel.Helper;

namespace Core.Application.ApplicationServices.Activities.Queries.GetById;

public record class GetActivityByIdQueryResponse(
    long Id,
    long? ParentId,
    Guid UserId,
    string Title,
    string? Description,
    DateTime StartDate,
    DateTime CreatedDate,
    DateTime UpdateDate,
    TimeSpan? Duration,
    ActivityCategory Category,
    bool IsCompleted
) : IResponse;
