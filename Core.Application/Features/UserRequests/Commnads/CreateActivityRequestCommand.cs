using Core.Application.DTOs.UserRequestDTOs;
using MediatR;

namespace Core.Application.Features.UserRequests.Commnads
{
    public record class CreateActivityRequestCommand(SendActivityRequest Request)
        : IRequest<string>;
}
