using Core.Application.ApplicationServices.Auth.Exceptions;
using Core.Application.InternalServices.Auth.Dtos;
using Core.Application.InternalServices.Auth.Services;
using MediatR;

namespace Core.Application.ApplicationServices.Auth.Commands.Login;

public class LoginCommandHandler(
    IUserSrevices serivces

    ) : IRequestHandler<LoginCommandRequest, LoginRequestResponse>
{

    private readonly IUserSrevices _userSrevices = serivces;

	public async Task<LoginRequestResponse> Handle(LoginCommandRequest request, CancellationToken cancellationToken)
    {
        var model = new LoginRequestDto(
            request.UserName,
            request.Password);

        var response = await _userSrevices.Login(model);
        if (response.IsFailed)
            throw new UserNotExistByUserNameAndPasswordException();

        return response.Value;
    }
}
