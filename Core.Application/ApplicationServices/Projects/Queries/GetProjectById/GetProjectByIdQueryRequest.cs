using MediatR;

namespace Core.Application.ApplicationServices.Projects.Queries.GetProjectById
{
    public record class GetProjectByIdQueryRequest(string Id) 
        : IRequest<GetProjectByIdQueryResponse>;
}
