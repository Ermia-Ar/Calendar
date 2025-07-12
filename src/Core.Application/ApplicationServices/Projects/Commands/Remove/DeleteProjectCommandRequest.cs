using MediatR;

namespace Core.Application.ApplicationServices.Projects.Commands.Remove;

public record class DeleteProjectCommandRequest(
    long ProjectId
    ) : IRequest;
