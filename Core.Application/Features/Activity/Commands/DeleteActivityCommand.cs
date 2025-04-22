using Core.Domain.Shared;
using MediatR;

namespace Core.Application.Features.Activity.Commands
{
    public class DeleteActivityCommand : IRequest<Response<string>>
    {
        public string Id { get; set; }
    }
}
