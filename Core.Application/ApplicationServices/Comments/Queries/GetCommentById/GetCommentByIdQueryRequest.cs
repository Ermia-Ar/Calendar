using MediatR;

namespace Core.Application.ApplicationServices.Comments.Queries.GetCommentById;

public record class GetCommentByIdQueryRequest(string Id)
    : IRequest<GetCommentByIdQueryResponse>;


