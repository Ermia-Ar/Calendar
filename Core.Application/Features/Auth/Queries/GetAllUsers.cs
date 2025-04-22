using Core.Application.DTOs.UserDTOs;
using Core.Domain.Shared;
using MediatR;

namespace Core.Application.Features.Auth.Queries
{
    public class GetAllUsers : IRequest<Response<List<UserResponse>>>
    {

    }
}
