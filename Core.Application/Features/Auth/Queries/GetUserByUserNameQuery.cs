using Core.Application.DTOs.UserDTOs;
using MediatR;

namespace Core.Application.Features.Auth.Queries
{
    public record class GetUserByUserNameQuery(string UserName)
        : IRequest<UserResponse>;
}
