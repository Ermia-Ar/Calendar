using MediatR;

namespace Core.Application.ApplicationServices.Activities.Queries.GetById;

public record class GetActivityByIdQueryRequest(long Id)
    : IRequest<GetActivityByIdQueryResponse>;

