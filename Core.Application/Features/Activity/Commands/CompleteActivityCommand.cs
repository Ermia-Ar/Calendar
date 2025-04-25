using Core.Domain.Shared;
using MediatR;

namespace Core.Application.Features.Activity.Commands
{
    public class CompleteActivityCommand : IRequest<Response<string>>
    {
        public string ActivityId { get; set; }  
    }
}
