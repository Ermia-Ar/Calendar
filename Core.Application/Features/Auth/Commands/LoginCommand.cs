using Core.Application.DTOs.AuthDTOs;
using Core.Domain.Helper;
using Core.Domain.Shared;
using MediatR;

namespace Core.Application.Features.Auth.Commands
{
    public class LoginCommand : IRequest<Response<JwtAuthResult>>
    {
        public LoginRequest LoginRequest { get; set; }
    }
}
