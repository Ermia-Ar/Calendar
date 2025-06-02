using Core.Application.DTOs.ProjectDTOs;
using MediatR;

namespace Core.Application.Features.Projects.Query
{
    public record class GetProjectsThatTheUserIsMemberOfQuery
        : IRequest<List<ProjectResponse>>
    {
    }
}
