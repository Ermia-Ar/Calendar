using Core.Application.DTOs.ProjectDTOs;
using Core.Domain.Shared;
using MediatR;

namespace Core.Application.Features.Projects.Query
{
    public class GettingProjectsOwnedByTheUserQuery : IRequest<Response<List<ProjectResponse>>>
    {
        
    }
}
