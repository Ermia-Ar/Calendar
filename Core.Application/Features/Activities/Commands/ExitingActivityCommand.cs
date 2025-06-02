using MediatR;

namespace Core.Application.Features.Activities.Commands
{
    public record class ExitingActivityCommand(string ActivityId) : IRequest<string>;
}
