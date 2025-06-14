using MediatR;

namespace Core.Application.ApplicationServices.Projects.Queries.GetMembers
{
    public record class GetMemberOfProjectQueryRequest(string ProjectId)
        : IRequest<List<GetMemberOfProjectQueryResponse>>;
}
