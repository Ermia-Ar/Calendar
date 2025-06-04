using MediatR;

namespace Core.Application.ApplicationServices.Projects.Queries.GetUserProjects
{
    public record class GetUserProjectsQueryRequest(
        DateTime? StartDate, 
        bool UserIsOwner,
        bool IsHistory)
        : IRequest<List<GetUserProjectQueryResponse>>;
}