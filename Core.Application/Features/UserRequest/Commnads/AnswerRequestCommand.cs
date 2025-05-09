using Core.Domain.Shared;
using MediatR;

namespace Core.Application.Features.UserRequests.Commnads
{
    public class AnswerRequestCommand : IRequest<Response<string>>
    {
        public string RequestId { get; set; }

        public bool IsAccepted { get; set; }
    }
}
