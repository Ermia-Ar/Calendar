using Core.Domain.Enum;
using MediatR;

namespace Core.Application.ApplicationServices.Auth.Queries.GetAll
{
    public record class GetAllUsersQueryRequest(string? Search, UserCategory? Category)
        : IRequest<List<GetAllUserQueryResponse>>;
}
