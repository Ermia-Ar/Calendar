using Core.Application.DTOs.UserRequestDTOs;
using Core.Domain.Enum;
using Core.Domain.Shared;
using MediatR;

namespace Core.Application.Features.UserRequests.Queries
{
    public class GetRequestsResponseQuery : IRequest<Response<List<ActivityRequestResponse>>>
    {
        public RequestFor RequestFor { get; set; }
    }
}
