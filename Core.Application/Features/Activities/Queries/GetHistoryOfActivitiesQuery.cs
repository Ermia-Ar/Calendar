using Core.Application.DTOs.ActivityDTOs;
using Core.Domain.Shared;
using MediatR;

namespace Core.Application.Features.Activities.Queries
{
    public class GetHistoryOfActivitiesQuery : IRequest<Response<List<ActivityResponse>>>
    {
    }
}
