using Core.Application.DTOs.ActivityDTOs;
using Core.Domain.Enum;
using Core.Domain.Shared;
using MediatR;

namespace Core.Application.Features.Activities.Queries
{
    public record class GetUserActivitiesQuery 
    (
        DateTime? StartDate,
        bool UserIsOwner,
        bool IsCompleted,
        bool IsHistory,
        ActivityCategory? Category
    )
        :IRequest<Response<List<ActivityResponse>>>;

}
