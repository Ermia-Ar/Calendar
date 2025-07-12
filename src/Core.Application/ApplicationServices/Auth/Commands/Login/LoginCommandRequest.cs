using Core.Application.InternalServices.Auth.Dtos;
using MediatR;

namespace Core.Application.ApplicationServices.Auth.Commands.Login;

/// <summary>
/// 
/// </summary>
/// <param name="UserName">نام کاربری وارد شود</param>
/// <param name="Password">حداقل 8 کاراکتر</param>
public record class LoginCommandRequest(
        string UserName,
        string Password

    ): IRequest<LoginRequestResponse>;
