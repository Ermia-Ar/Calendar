using Core.Application.DTOs.ActivityDTOs;
using Core.Domain.Shared;
using MediatR;

namespace Core.Application.Features.Activity.Commands
{
    public class CreateActivityCommand : IRequest<Response<string>>
    {
        public CreateActivityRequest createActivityRequest { get; set; }
    }
}
