using MediatR;

namespace Core.Application.ApplicationServices.UserRequests.Queries.GetById
{
    public sealed record GetRequestByIdQueryRequest(
        string Id

        ) : IRequest<GetRequestByIdQueryResponse>;
}
