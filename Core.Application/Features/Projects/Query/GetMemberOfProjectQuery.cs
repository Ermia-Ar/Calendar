using Core.Application.DTOs.UserDTOs;
using Core.Domain.Shared;
using MediatR;

namespace Core.Application.Features.Projects.Query
{
    public class GetMemberOfProjectQuery : IRequest<Response<List<UserResponse>>>
    {
        public string ProjectId { get; set; }   
    }
}
