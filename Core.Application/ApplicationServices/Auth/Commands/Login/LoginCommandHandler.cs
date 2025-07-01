using Core.Application.ApplicationServices.Auth.Exceptions;
using Core.Application.ExternalServices.Jwt;
using Core.Domain.Entities.Users;
using Core.Domain.UnitOfWork;
using MediatR;
using Share.Utility;

namespace Core.Application.ApplicationServices.Auth.Commands.Login;

public class LoginCommandHandler
    : IRequestHandler<LoginCommandRequest, string>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITokenServices _tokenServices;

    public LoginCommandHandler(IUnitOfWork unitOfWork,
        ITokenServices tokenServices)
    {
        _unitOfWork = unitOfWork;
        _tokenServices = tokenServices;
    }

    public async Task<string> Handle(LoginCommandRequest request, CancellationToken cancellationToken)
    {
        User? user;
        //check email or user name
        if (request.UserNameOrEmail.IsEmail())
            user = await _unitOfWork.Users.FindByEmail(request.UserNameOrEmail);
        else
            user = await _unitOfWork.Users.FindByUserName(request.UserNameOrEmail);

        if (user == null)
        {
            throw new UserNotExistByUserNameAndPasswordException();
        }
        // check password 
        bool checkPassword = await _unitOfWork.Users.CheckPasswordAsync(user, request.Password);
        if (!checkPassword)
        {
            throw new UserNotExistByUserNameAndPasswordException();
        }
        return await _tokenServices.GetJWTToken(user);
    }
}
