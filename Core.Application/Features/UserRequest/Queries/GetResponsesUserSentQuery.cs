using Core.Application.DTOs.UserRequestDTOs;
using Core.Domain.Shared;
using MediatR;

namespace Core.Application.Features.UserRequests.Queries
{
    public class GetResponsesUserSentQuery : IRequest<Response<List<ActivityRequestResponse>>>
    {
    }
}
