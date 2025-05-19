using Core.Application.DTOs.ActivityDTOs;
using Core.Domain.Shared;
using MediatR;

namespace Core.Application.Features.Activities.Queries
{
    public record class GetActivitiesThatTheUserIsMemberOfQuery 
                : IRequest<Response<List<ActivityResponse>>>;
}
