using Core.Application.DTOs.UserDTOs;
using MediatR;

namespace Core.Application.Features.Projects.Query
{
    public record class GetMemberOfProjectQuery(string ProjectId)
        : IRequest<List<UserResponse>>;
}
