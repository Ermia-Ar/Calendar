using Core.Application.DTOs.ActivityDTOs;
using Core.Domain.Shared;
using MediatR;

namespace Core.Application.Features.Activities.Commands
{
    public class CreateActivityCommand : IRequest<Response<string>>
    {
        public CreateActivityRequest createActivityRequest { get; set; }
    }
}
