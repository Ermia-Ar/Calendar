using MediatR;

namespace Core.Application.ApplicationServices.Auth.Commands.Login;

/// <summary>
/// 
/// </summary>
/// <param name="UserNameOrEmail">نام کاربری یا ایمیل وارد شود</param>
/// <param name="Password">حداقل 8 کاراکتر</param>
public record class LoginCommandRequest(
        string UserNameOrEmail,
        string Password

    ): IRequest<string>;
