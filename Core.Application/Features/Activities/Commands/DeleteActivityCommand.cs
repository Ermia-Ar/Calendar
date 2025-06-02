using MediatR;

namespace Core.Application.Features.Activities.Commands
{
    public record class DeleteActivityCommand(string Id) : IRequest<string>;
}
