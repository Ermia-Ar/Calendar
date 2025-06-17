using MediatR;

namespace Core.Application.ApplicationServices.Auth.Queries.GetByUserName;

public sealed record GetUserByUserNameQueryRequest
(
    string UserName
):IRequest<GetUserByUserNameQueryResponse>;
