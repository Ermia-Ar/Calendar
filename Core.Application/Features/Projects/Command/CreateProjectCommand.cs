using Core.Application.DTOs.ProjectDTOs;
using Core.Domain.Shared;
using MediatR;

namespace Core.Application.Features.Projects.Command
{
    public class CreateProjectCommand : IRequest<Response<string>>
    {
        public CreateProjectRequest CreateProject { get; set; }
    }
}
