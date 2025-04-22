using Core.Application.DTOs.UserDTOs;
using Core.Domain.Shared;
using MediatR;

namespace Core.Application.Features.Auth.Queries
{
    public class GetUserByUserNameQuery : IRequest<Response<UserResponse>>
    {
        public string UserName { get; set; }
    }
}
