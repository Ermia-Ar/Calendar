using Core.Application.ApplicationServices.Activities.Queries.GetById;
using Core.Domain.Enum;
using MediatR;

namespace Core.Application.ApplicationServices.Activities.Queries.GetUserActivities
{
    public record class GetUserActivitiesQuery
    (
        DateTime? StartDate,
        bool UserIsOwner,
        bool IsCompleted,
        bool IsHistory,
        ActivityCategory? Category
    )
        : IRequest<List<GetByIdActivityQueryResponse>>;

}
