using Core.Application.DTOs.ProjectDTOs;
using MediatR;

namespace Core.Application.Features.Projects.Command
{
    public record class CreateProjectCommand(CreateProjectRequest CreateProject)
        : IRequest<string>;
}
