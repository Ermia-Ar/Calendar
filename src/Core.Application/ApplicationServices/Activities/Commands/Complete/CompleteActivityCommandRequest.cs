using MediatR;

namespace Core.Application.ApplicationServices.Activities.Commands.Complete
{
    public record class CompleteActivityCommandRequest(
        long ActivityId
        ) : IRequest;
}
