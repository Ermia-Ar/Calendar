using Core.Application.DTOs.AuthDTOs;
using MediatR;

namespace Core.Application.Features.Auth.Commands
{
    public record class LoginCommand(LoginRequest LoginRequest)
        : IRequest<string>;
}
