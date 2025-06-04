using MediatR;

namespace Core.Application.ApplicationServices.Comments.Commands.DeleteComment;

public record class DeleteCommentCommandRequest(
    string Id
    ) : IRequest;
