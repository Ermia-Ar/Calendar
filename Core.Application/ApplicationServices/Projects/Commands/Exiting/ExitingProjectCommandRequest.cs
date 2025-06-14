using MediatR;

namespace Core.Application.ApplicationServices.Projects.Commands.Exiting;

public record class ExitingProjectCommandRequest(
    string ProjectId
    ): IRequest;
