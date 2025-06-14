using MediatR;

namespace Core.Application.ApplicationServices.UserRequests.Queries.GetById
{
    public record class GetRequestByIdQueryRequest(
        string Id

        ) : IRequest<GetRequestByIdQueryResponse>;
}
