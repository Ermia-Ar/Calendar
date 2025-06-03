using MediatR;

namespace Core.Application.ApplicationServices.Activities.Queries.GetById;

public record class GetActivityByIdQueryRequest(string Id)
    : IRequest<GetByIdActivityQueryResponse>;

