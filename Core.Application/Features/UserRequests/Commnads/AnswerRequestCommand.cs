using MediatR;

namespace Core.Application.Features.UserRequests.Commnads
{
    public record class AnswerRequestCommand(string RequestId, bool IsAccepted)
        : IRequest<string>;
}
