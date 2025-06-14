using MediatR;

namespace Core.Application.ApplicationServices.Activities.Commands.Remove
{
    public record class DeleteActivityCommandRequest(
        string Id
        ) : IRequest;
}
