using MediatR;

namespace Core.Application.ApplicationServices.Comments.Queries.GetById;

public record class GetCommentByIdQueryRequest(string Id)
    : IRequest<GetCommentByIdQueryResponse>;


