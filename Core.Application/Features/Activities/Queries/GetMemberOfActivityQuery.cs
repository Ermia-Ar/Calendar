using Core.Application.DTOs.UserDTOs;
using Core.Domain.Shared;
using MediatR;

namespace Core.Application.Features.Activities.Queries
{
    public class GetMemberOfActivityQuery : IRequest<Response<List<string>>>
    {
        public string ActivityId { get; set; }
    }
}
