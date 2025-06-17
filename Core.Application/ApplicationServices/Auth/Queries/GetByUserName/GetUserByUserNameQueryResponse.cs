using Core.Domain.Enum;

namespace Core.Application.ApplicationServices.Auth.Queries.GetByUserName;

public sealed record GetUserByUserNameQueryResponse(
    string Id,
    string UserName,
    string Email,
    UserCategory Category
    );