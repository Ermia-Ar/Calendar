using Core.Domain.Shared;
using MediatR;

namespace Core.Application.Features.Activities.Commands
{
    public class RemoveMemberOfActivityCommand : IRequest<Response<string>>
    {
        public string ActivityId { get; set; }

        public string UserName { get; set; }
    }

}
