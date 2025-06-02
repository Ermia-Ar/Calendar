using Core.Application.DTOs.ProjectDTOs;
using MediatR;

namespace Core.Application.Features.Projects.Query
{
    public class GettingProjectsOwnedByTheUserQuery : IRequest<List<ProjectResponse>>
    {

    }
}
