using Core.Application.DTOs.ActivityDTOs;
using Core.Domain.Shared;
using MediatR;

namespace Core.Application.Features.Activities.Commands
{
    public record class CreateActivityCommand(CreateActivityRequest CreateActivity)
        : IRequest<Response<string>>;
}
