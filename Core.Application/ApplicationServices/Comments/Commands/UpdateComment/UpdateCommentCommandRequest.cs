using MediatR;

namespace Core.Application.ApplicationServices.Comments.Commands.UpdateComment;

public record class UpdateCommentCommandRequest(
    string Id,
    string Content)
    : IRequest;
