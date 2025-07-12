using Core.Application.ApplicationServices.Requests.Queries.GetAll;
using MediatR;

namespace Core.Application.ApplicationServices.Requests.Commands.Answer;

/// <summary>
/// 
/// </summary>
/// <param name="RequestId"></param>
/// <param name="IsAccepted"></param>
public sealed record AnswerRequestCommandRequest(
    long RequestId,
    bool IsAccepted)
    : IRequest
{
    public static AnswerRequestCommandRequest Create(long id, AnswerRequestDto model)
        => new AnswerRequestCommandRequest(id, model.IsAccepted);
}



/// <summary>
/// 
/// </summary>
/// <param name="IsAccepted"></param>
public sealed record AnswerRequestDto(
	bool IsAccepted
	);