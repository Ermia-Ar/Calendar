using Core.Application.DTOs.UserRequestDTOs;
using Core.Domain.Enum;
using MediatR;

namespace Core.Application.Features.UserRequests.Queries
{
    public record class GetRequestsReceivedQuery(RequestFor? RequestFor)
        : IRequest<List<RequestResponse>>;
}
