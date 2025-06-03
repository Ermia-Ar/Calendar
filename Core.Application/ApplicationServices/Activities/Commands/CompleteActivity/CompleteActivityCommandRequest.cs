using MediatR;

namespace Core.Application.ApplicationServices.Activities.Commands.CompleteActivity
{
    public record class CompleteActivityCommandRequest(
        string ActivityId
        ) : IRequest;
}
