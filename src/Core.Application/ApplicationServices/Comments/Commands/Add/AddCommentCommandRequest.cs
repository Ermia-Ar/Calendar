using Core.Application.ApplicationServices.Comments.Queries.GetAll;
using MediatR;

namespace Core.Application.ApplicationServices.Comments.Commands.Add;

/// <summary>
/// 
/// </summary>
/// <param name="ActivityId"></param>
/// <param name="Content">متن کامنت</param>
public record class AddCommentCommandRequest(
    long ActivityId,
    string Content)
    : IRequest;
