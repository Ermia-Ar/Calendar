using MediatR;

namespace Core.Application.ApplicationServices.Projects.Commands.DeleteProject;

public record class DeleteProjectCommandRequest(
    string ProjectId
    ): IRequest;
