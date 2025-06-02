using MediatR;

namespace Core.Application.Features.Activities.Commands
{
    public record class CompleteActivityCommand(string ActivityId) : IRequest<string>;
}
