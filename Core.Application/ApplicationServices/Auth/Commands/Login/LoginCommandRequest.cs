using MediatR;

namespace Core.Application.ApplicationServices.Auth.Commands.Login;

public record class LoginCommandRequest(

        string UserNameOrEmail,
        string Password

    ): IRequest;
