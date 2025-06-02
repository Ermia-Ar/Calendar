using Core.Application.Exceptions.User;
using Core.Application.Features.Auth.Commands;
using Core.Application.Services;
using Core.Domain;
using Core.Domain.Entity;
using MediatR;
using Share.Utility;

namespace Core.Application.Features.Auth.Handler
{
    public class LoginHandler
        : IRequestHandler<LoginCommand, string>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenServices _tokenServices;

        public LoginHandler(IUnitOfWork unitOfWork,
            ITokenServices tokenServices)
        {
            _unitOfWork = unitOfWork;
            _tokenServices = tokenServices;
        }

        public async Task<string> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            User? user;
            //check email or user name
            if (Utilities.IsEmail(request.LoginRequest.UserNameOrEmail))
                user = await _unitOfWork.Users.FindByEmail(request.LoginRequest.UserNameOrEmail);
            else
                user = await _unitOfWork.Users.FindByUserName(request.LoginRequest.UserNameOrEmail);

            if (user == null)
            {
                throw new UserNotExistByUserNameAndPasswordException();
            }
            // check password 
            bool checkPassword = await _unitOfWork.Users.CheckPasswordAsync(user, request.LoginRequest.Password);
            if (!checkPassword)
            {
                throw new UserNotExistByUserNameAndPasswordException();
            }
            var token = await _tokenServices.GetJWTToken(user);

            return token;
        }
    }
}
