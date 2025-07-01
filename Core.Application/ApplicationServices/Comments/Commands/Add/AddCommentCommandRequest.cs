using Core.Application.ApplicationServices.Comments.Queries.GetAll;
using MediatR;

namespace Core.Application.ApplicationServices.Comments.Commands.Add;

public record class AddCommentCommandRequest(
    string ActivityId,
    string Content)
    : IRequest<GetAllCommentsQueryResponse>;
