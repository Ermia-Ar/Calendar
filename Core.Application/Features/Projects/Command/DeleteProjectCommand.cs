using MediatR;

namespace Core.Application.Features.Projects.Command
{
    public record class DeleteProjectCommand(string ProjcetId)
        : IRequest<string>;
}
