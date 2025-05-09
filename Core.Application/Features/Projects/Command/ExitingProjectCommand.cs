using Core.Domain.Shared;
using MediatR;

namespace Core.Application.Features.Projects.Command
{
    public class ExitingProjectCommand : IRequest<Response<string>>
    {
        public string ProjectId { get; set; }
    }
}
