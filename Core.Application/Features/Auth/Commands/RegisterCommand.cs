using Core.Application.DTOs.AuthDTOs;
using MediatR;

namespace Core.Application.Features.Auth.Commands
{
    public record class RegisterCommand(RegisterRequest RegisterRequest)
        : IRequest<string>;
}
