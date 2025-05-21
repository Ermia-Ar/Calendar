using Core.Application.DTOs.AuthDTOs;
using Core.Domain.Helper;
using Core.Domain.Shared;
using MediatR;

namespace Core.Application.Features.Auth.Commands
{
    public class RegisterCommand : IRequest<Response<JwtAuthResult>>
    {
        public RegisterRequest RegisterRequest { get; set; }
    }
}
