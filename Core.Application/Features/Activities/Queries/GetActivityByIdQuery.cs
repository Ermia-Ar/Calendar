using Core.Application.DTOs.ActivityDTOs;
using MediatR;

namespace Core.Application.Features.Activities.Queries
{
    public record class GetActivityByIdQuery(string Id) 
        : IRequest<ActivityResponse>;
}
