using MediatR;

namespace Core.Application.ApplicationServices.Projects.Queries.GetMemberOfProject
{
    public record class GetMemberOfProjectQueryRequest(string ProjectId)
        : IRequest<List<GetMemberOfProjectQueryResponse>>;
}
