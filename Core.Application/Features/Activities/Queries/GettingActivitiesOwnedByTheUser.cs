using Core.Application.DTOs.ActivityDTOs;
using Core.Domain.Shared;
using MediatR;

namespace Core.Application.Features.Activities.Queries
{
    public record class GettingActivitiesOwnedByTheUser : IRequest<Response<List<ActivityResponse>>>;
}
