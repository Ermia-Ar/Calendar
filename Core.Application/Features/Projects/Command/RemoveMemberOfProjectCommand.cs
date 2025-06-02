using MediatR;

namespace Core.Application.Features.Projects.Command
{
    public record class RemoveMemberOfProjectCommand(string ProjectId, string UserName)
        : IRequest<string>;
}
