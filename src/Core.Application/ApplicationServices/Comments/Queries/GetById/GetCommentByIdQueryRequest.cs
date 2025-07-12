using MediatR;

namespace Core.Application.ApplicationServices.Comments.Queries.GetById;

public record class GetCommentByIdQueryRequest(long Id)
    : IRequest<GetCommentByIdQueryResponse>;


