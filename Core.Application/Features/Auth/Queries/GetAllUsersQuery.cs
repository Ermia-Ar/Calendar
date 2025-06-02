using Core.Application.DTOs.UserDTOs;
using Core.Domain.Enum;
using MediatR;

namespace Core.Application.Features.Auth.Queries
{
    public record class GetAllUsersQuery(string? Search, UserCategory? Category)
        : IRequest<List<UserResponse>>;
}
