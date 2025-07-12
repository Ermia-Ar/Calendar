using Core.Application.ApplicationServices.Comments.Queries.GetAll;
using MediatR;

namespace Core.Application.ApplicationServices.Comments.Commands.Update;

public record class UpdateCommentCommandRequest(
    long Id,
    string Content)
    : IRequest;
