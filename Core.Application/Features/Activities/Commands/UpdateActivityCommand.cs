using Core.Application.DTOs.ActivityDTOs;
using MediatR;

namespace Core.Application.Features.Activities.Commands
{
    public record class UpdateActivityCommand(UpdateActivityRequest UpdateActivityRequest)
        : IRequest<string>;
}
