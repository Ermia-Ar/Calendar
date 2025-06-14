using MediatR;

namespace Core.Application.ApplicationServices.Comments.Commands.Add;

public record class AddCommentCommandRequest(
    string ProjectId,
    string ActivityId,
    string Content)
    : IRequest;
