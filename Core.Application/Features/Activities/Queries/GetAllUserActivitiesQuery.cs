using Core.Application.DTOs.ActivityDTOs;
using Core.Domain.Shared;
using MediatR;

namespace Core.Application.Features.Activities.Queries
{
    public class GetAllUserActivitiesQuery : IRequest<Response<List<ActivityResponse>>>
    {
    }
}
