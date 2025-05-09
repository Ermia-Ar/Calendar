using Core.Domain.Shared;
using MediatR;

namespace Core.Application.Features.Activities.Commands
{
    public class DeleteActivityCommand : IRequest<Response<string>>
    {
        public string Id { get; set; }
    }
}
