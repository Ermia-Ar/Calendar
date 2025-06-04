using MediatR;

namespace Core.Application.ApplicationServices.UserRequests.Queries.GetRequestById
{
    public record class GetRequestByIdQueryRequest(
        string Id

        ) : IRequest<GetRequestByIdQueryResponse>;
}
