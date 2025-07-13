using MediatR;

namespace Core.Application.ApplicationServices.Projects.Commands.Update;

public sealed record UpdateProjectCommandRequest(
    long ProjectId,
    string Title,
    string Description,
    DateTime StartDate,
    DateTime EndDate
) : IRequest
{
    public static UpdateProjectCommandRequest Create(long projectId, UpdateProjectDto model)
        => new UpdateProjectCommandRequest(projectId, model.Title,
            model.Description, model.StartDate, model.EndDate);
}




public sealed record UpdateProjectDto(
    string Title,
    string Description,
    DateTime StartDate,
    DateTime EndDate
    );