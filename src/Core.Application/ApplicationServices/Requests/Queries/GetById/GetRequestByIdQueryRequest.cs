using MediatR;

namespace Core.Application.ApplicationServices.Requests.Queries.GetById
{
    public sealed record GetRequestByIdQueryRequest(
        long Id

        ) : IRequest<GetRequestByIdQueryResponse>;
}
