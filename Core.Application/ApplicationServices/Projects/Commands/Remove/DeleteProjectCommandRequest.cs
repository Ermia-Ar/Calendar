using MediatR;

namespace Core.Application.ApplicationServices.Projects.Commands.Remove;

public record class DeleteProjectCommandRequest(
    string ProjectId
    ): IRequest;
