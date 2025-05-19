using Core.Application.DTOs.ActivityDTOs;
using Core.Domain.Shared;
using MediatR;

namespace Core.Application.Features.Activities.Commands
{
    public record class UpdateActivityCommand(UpdateActivityRequest UpdateActivityRequest) 
        : IRequest<Response<ActivityResponse>>;
}
