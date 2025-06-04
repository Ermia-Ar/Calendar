using MediatR;

namespace Core.Application.ApplicationServices.Projects.Commands.ExitingProject;

public record class ExitingProjectCommandRequest(
    string ProjectId
    ): IRequest;
