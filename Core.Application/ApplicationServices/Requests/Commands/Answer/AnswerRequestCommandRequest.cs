using Core.Application.ApplicationServices.Requests.Queries.GetAll;
using MediatR;

namespace Core.Application.ApplicationServices.Requests.Commands.Answer;

public record class AnswerRequestCommandRequest(
    string RequestId,
    bool IsAccepted)
    : IRequest<GetAllRequestQueryResponse>;