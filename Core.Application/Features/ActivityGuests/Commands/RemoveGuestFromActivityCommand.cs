using Core.Domain.Shared;
using MediatR;

namespace Core.Application.Features.ActivityGuests.Commands
{
    public class RemoveGuestFromActivityCommand : IRequest<Response<string>>
    {
        public string ActivityId { get; set; }
        public string UserId { get; set; }
    }
}
