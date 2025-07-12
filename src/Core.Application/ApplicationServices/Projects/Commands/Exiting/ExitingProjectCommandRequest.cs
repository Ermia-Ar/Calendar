using MediatR;

namespace Core.Application.ApplicationServices.Projects.Commands.Exiting;

public record class ExitingProjectCommandRequest(
    long ProjectId,
    bool Activities
    ) : IRequest;
