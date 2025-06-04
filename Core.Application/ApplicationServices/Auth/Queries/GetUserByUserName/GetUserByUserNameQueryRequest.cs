using MediatR;

namespace Core.Application.ApplicationServices.Auth.Queries.GetUserByUserName;

public record class GetUserByUserNameQueryRequest(string UserName)
    : IRequest<GetUserByUserNameQueryResponse>;
