using MediatR;

namespace Core.Application.ApplicationServices.Projects.Queries.GetAll
{
    public record class GetUserProjectsQueryRequest(
        DateTime? StartDate, 
        bool UserIsOwner,
        bool IsHistory)
        : IRequest<List<GetUserProjectQueryResponse>>;
}