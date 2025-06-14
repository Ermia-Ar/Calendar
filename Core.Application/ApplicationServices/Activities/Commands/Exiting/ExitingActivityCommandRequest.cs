using MediatR;

namespace Core.Application.ApplicationServices.Activities.Commands.Exiting;

public record class ExitingActivityCommandRequest(
    string ActivityId
    ) : IRequest;
