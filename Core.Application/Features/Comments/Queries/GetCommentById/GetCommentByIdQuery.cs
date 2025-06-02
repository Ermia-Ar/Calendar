using Core.Application.Features.Comments.Queries.GetComments;
using Core.Domain.Entity;
using MediatR;

namespace Core.Application.Features.Comments.Queries.GetCommentById
{
    public record class GetCommentByIdQuery(string Id ) 
        : IRequest<GetCommentsResponse>;
}
