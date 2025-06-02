using Core.Application.DTOs.ProjectDTOs;
using MediatR;

namespace Core.Application.Features.Projects.Query
{
    public record class GetUserProjectsQuery(DateTime? StartDate, bool UserIsOwner, bool IsHistory)
        : IRequest<List<ProjectResponse>>;
}