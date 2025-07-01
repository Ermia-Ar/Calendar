using MediatR;

namespace Core.Application.ApplicationServices.Requests.Queries.GetById
{
    public sealed record GetRequestByIdQueryRequest(
        string Id

        ) : IRequest<GetRequestByIdQueryResponse>;
}
