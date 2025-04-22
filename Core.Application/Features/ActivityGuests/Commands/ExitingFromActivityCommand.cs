using Core.Domain.Shared;
using MediatR;

namespace Core.Application.Features.ActivityGuests.Commands
{
    public class ExitingFromActivityCommand : IRequest<Response<string>>
    {
        public string ActivityId { get; set;}
    }
}
