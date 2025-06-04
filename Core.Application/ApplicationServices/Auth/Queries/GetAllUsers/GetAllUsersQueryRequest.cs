using Core.Domain.Enum;
using MediatR;

namespace Core.Application.ApplicationServices.Auth.Queries.GetAllUsers
{
    public record class GetAllUsersQueryRequest(string? Search, UserCategory? Category)
        : IRequest<List<GetAllUserQueryResponse>>;
}
