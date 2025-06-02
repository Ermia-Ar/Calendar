using MediatR;

namespace Core.Application.Features.Comments.Commands.CreateComment
{
    public record class CreateCommentCommand(string ProjectId, string ActivityId, string Content)
        : IRequest<string>;
}
