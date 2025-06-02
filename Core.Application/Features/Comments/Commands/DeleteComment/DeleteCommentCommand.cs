using MediatR;

namespace Core.Application.Features.Comments.Commands.DeleteComment
{
    public record class DeleteCommentCommand(string Id) : IRequest<string>;
}
