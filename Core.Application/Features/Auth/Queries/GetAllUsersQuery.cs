using Core.Application.DTOs.UserDTOs;
using Core.Domain.Enum;
using Core.Domain.Shared;
using MediatR;

namespace Core.Application.Features.Auth.Queries
{
    public class GetAllUsersQuery : IRequest<Response<List<UserResponse>>>
    {
        public string? Search { get; set; }
        public UserCategory? Category { get; set; }
    }
}
