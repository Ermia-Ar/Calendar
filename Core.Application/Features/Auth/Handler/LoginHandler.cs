using Core.Application.Features.Auth.Commands;
using Core.Application.Utility;
using Core.Domain;
using Core.Domain.Entity;
using Core.Domain.Helper;
using Core.Domain.Interfaces;
using Core.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Core.Application.Features.Auth.Handler
{
    public class LoginHandler : ResponseHandler
        , IRequestHandler<LoginCommand, Response<JwtAuthResult>>
    {
        private IUnitOfWork _unitOfWork;
        private ITokenServices _tokenServices;

        public LoginHandler(IUnitOfWork unitOfWork,
            ITokenServices tokenServices)
        {
            _unitOfWork = unitOfWork;
            _tokenServices = tokenServices;
        }

        public async Task<Response<JwtAuthResult>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            User user;
            //check email or user name
            if (Utilities.IsEmail(request.LoginRequest.UserNameOrEmail))
            {
                user = await _unitOfWork.Users.FindByEmail(request.LoginRequest.UserNameOrEmail);
            }
            else
            {
                user = await _unitOfWork.Users.FindByUserName(request.LoginRequest.UserNameOrEmail);
            }
            if (user == null)
            {
                return BadRequest<JwtAuthResult>("UserName or Password is wrong");
            }
            // check password 
            bool checkPassword = await _unitOfWork.Users.CheckPasswordAsync(user, request.LoginRequest.Password);
            if (!checkPassword)
            {
                return BadRequest<JwtAuthResult>("UserName or Password is wrong");
            }
            var token = await _tokenServices.GetJWTToken(user);

            return Success(token);
        }


    }

    
}
