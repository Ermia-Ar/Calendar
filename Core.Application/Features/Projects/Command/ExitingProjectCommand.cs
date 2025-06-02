using MediatR;

namespace Core.Application.Features.Projects.Command
{
    public record class ExitingProjectCommand(string ProjectId)
        : IRequest<string>;
}
