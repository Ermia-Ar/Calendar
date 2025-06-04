using MediatR;

namespace Core.Application.ApplicationServices.UserRequests.Commands.AnswerRequest;

public record class AnswerRequestCommandRequest(
    string RequestId,
    bool IsAccepted)
    : IRequest;