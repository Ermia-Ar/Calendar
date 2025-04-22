using Core.Domain.Shared;
using MediatR;

namespace Core.Application.Features.UserRequest.Commnads
{
    public class AnswerRequestCommand : IRequest<Response<string>>
    {
        public string RequestId { get; set; }

        public bool IsAccepted { get; set; }
    }
}
