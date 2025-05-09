using Core.Domain.Shared;
using MediatR;

namespace Core.Application.Features.Activities.Commands
{
    public class ExitingActivityCommand : IRequest<Response<string>>
    {
        public string ActivityId { get; set; }
    }

}
