using Core.Application.ApplicationServices.Comments.Queries.GetAll;
using MediatR;

namespace Core.Application.ApplicationServices.Comments.Commands.Update;

public record class UpdateCommentCommandRequest(
    string Id,
    string Content)
    : IRequest<GetAllCommentsQueryResponse>;
