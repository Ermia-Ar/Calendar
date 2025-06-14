using MediatR;

namespace Core.Application.ApplicationServices.Auth.Queries.GetById;

public record class GetUserByIdQueryRequest(string Id)
    : IRequest<GetUserByIdQueryResponse>;
