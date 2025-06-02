using Core.Application.DTOs.UserRequestDTOs;
using Core.Domain.Enum;
using MediatR;

namespace Core.Application.Features.UserRequests.Queries
{
    public record class GetResponsesUserSentQuery(RequestFor? RequestFor)
        : IRequest<List<RequestResponse>>;
}
