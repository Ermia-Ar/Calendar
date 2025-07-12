using MediatR;

namespace Core.Application.ApplicationServices.Projects.Queries.GetById
{
    public record class GetProjectByIdQueryRequest(long Id)
        : IRequest<GetProjectByIdQueryResponse>;
}
