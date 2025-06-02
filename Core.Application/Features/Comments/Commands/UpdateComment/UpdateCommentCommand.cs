using MediatR;

namespace Core.Application.Features.Comments.Commands.UpdateComment
{
    public record class UpdateCommentCommand(string Id, string Content) : IRequest<string>;
}
