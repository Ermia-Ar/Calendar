using Core.Application.DTOs.ActivityDTOs;
using MediatR;

namespace Core.Application.Features.Activities.Commands
{
    public record class CreateActivityCommand(CreateActivityRequest CreateActivity)
        : IRequest<string>;
}
