using MediatR;

namespace Core.Application.ApplicationServices.Comments.Commands.Remove;

public record class DeleteCommentCommandRequest(
    long Id
    ) : IRequest;
