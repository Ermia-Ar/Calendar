using Core.Application.DTOs.ActivityDTOs;
using MediatR;

namespace Core.Application.Features.Projects.Query
{
    public record class GetActivitiesOfProjectQuery(string ProjectId)
        : IRequest<List<ActivityResponse>>;
}
