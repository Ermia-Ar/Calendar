using Core.Application.ApplicationServices.Requests.Queries.GetAll;
using MediatR;

namespace Core.Application.ApplicationServices.Requests.Commands.Answer;

/// <summary>
/// 
/// </summary>
/// <param name="RequestId"></param>
/// <param name="IsAccepted"></param>
public record class AnswerRequestCommandRequest(
    string RequestId,
    bool IsAccepted)
    : IRequest<GetAllRequestQueryResponse>;