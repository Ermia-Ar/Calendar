using Core.Application.DTOs.UserDTOs;
using MediatR;

namespace Core.Application.ApplicationServices.Activities.Queries.GetMemberOfActivity
{
    public record class GetMemberOfActivityQuery(string ActivityId) : IRequest<List<UserResponse>>;
}
