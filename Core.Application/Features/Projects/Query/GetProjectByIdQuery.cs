using Core.Application.DTOs.ProjectDTOs;
using MediatR;

namespace Core.Application.Features.Projects.Query
{
    public record class GetProjectByIdQuery(string Id) : IRequest<ProjectResponse>
    {
    }
}
