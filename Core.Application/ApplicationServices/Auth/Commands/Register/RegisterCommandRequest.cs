using Core.Domain.Enum;
using MediatR;

namespace Core.Application.ApplicationServices.Auth.Commands.Register;

public record class RegisterCommandRequest(
        string UserName,
        string Email,
        string Password,
        UserCategory Category

    ) : IRequest;
