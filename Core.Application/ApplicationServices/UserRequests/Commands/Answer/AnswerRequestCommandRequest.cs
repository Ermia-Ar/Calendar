using MediatR;

namespace Core.Application.ApplicationServices.UserRequests.Commands.Answer;

public record class AnswerRequestCommandRequest(
    string RequestId,
    bool IsAccepted)
    : IRequest;