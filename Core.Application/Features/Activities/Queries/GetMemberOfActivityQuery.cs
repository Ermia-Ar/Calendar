using Core.Application.DTOs.UserDTOs;
using Core.Domain.Shared;
using MediatR;

namespace Core.Application.Features.Activities.Queries
{
    public record class GetMemberOfActivityQuery(string ActivityId) : IRequest<Response<List<UserResponse>>>;
}
