using Core.Application.DTOs.ActivityDTOs;
using Core.Domain.Shared;
using MediatR;

namespace Core.Application.Features.Activities.Commands
{
    public class UpdateActivityCommand : IRequest<Response<ActivityResponse>>
    {
        public UpdateActivityRequest UpdateActivityRequest { get; set; }
    }

}
