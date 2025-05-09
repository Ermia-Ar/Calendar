using Core.Domain.Shared;
using MediatR;

namespace Core.Application.Features.UserRequests.Commnads
{
    public class DeleteRequestCommand : IRequest<Response<string>>
    {
        public string Id { get; set; }
    }
}
