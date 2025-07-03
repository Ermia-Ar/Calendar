using Core.Domain.Enum;
using MediatR;

namespace Core.Application.ApplicationServices.Auth.Commands.Register;


/// <summary>
/// 
/// </summary>
/// <param name="UserName">باید غیر نکراری باشد و حداقل 3 کاراکتر</param>
/// <param name="Email">باید غیر نکراری باشد</param>
/// <param name="Password">حداقل 8 کاراکتر</param>
/// <param name="Category"></param>
public sealed record RegisterCommandRequest(
        string UserName,
        string Email,
        string Password,
        UserCategory Category

    ) : IRequest;
