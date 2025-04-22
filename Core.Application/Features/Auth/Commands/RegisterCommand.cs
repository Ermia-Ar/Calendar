using Core.Application.DTOs.AuthDTOs;
using Core.Domain.Shared;
using MediatR;

namespace Core.Application.Features.Auth.Commands
{
    public class RegisterCommand : IRequest<Response<string>>
    {
        public RegisterRequest RegisterRequest { get; set; }
    }
}
