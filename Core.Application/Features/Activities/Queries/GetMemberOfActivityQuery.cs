using Core.Application.DTOs.UserDTOs;
using MediatR;

namespace Core.Application.Features.Activities.Queries
{
    public record class GetMemberOfActivityQuery(string ActivityId) : IRequest<List<UserResponse>>;
}
