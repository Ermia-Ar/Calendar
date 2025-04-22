using Core.Application.DTOs.AuthDTOs;
using Core.Application.DTOs.UserDTOs;
using Core.Application.Features.Auth.Commands;
using Core.Application.Features.Auth.Queries;
using Core.Application.Interfaces;
using Core.Domain.Shared;
using MediatR;

namespace Core.Application.Features.Auth.Handler
{
    public class AuthHandler : ResponseHandler
        , IRequestHandler<RegisterCommand, Response<string>>
        , IRequestHandler<LoginCommand, Response<JwtAuthResult>>
        , IRequestHandler<GetUserByUserNameQuery, Response<UserResponse>>
        , IRequestHandler<GetAllUsers, Response<List<UserResponse>>>
    {
        private IAuthServices _authServices;
        private ITokenServices _tokenServices;

        public AuthHandler(IAuthServices authServices, ITokenServices tokenServices)
        {
            _authServices = authServices;
            _tokenServices = tokenServices;
        }

        public async Task<Response<string>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var result = await _authServices.Register(request.RegisterRequest);

            if (result.IsFailure)
            {
                return UnProcessableEntity<string>(result.Error.Message);
            }
            return NoContent<string>("User created successfully.");
        }

        public async Task<Response<JwtAuthResult>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var result = await _authServices.Login(request.LoginRequest);
            if (result.IsFailure)
            {
                return BadRequest<JwtAuthResult>(result.Error.Message);
            }
            var token = await _tokenServices.GetJWTToken(request.LoginRequest.UserNameOrEmail);

            return Success(token);
        }

        public async Task<Response<UserResponse>> Handle(GetUserByUserNameQuery request, CancellationToken cancellationToken)
        {
            var result = await _authServices.GetUserByUserName(request.UserName);

            if (result.IsFailure)
            {
                return NotFound<UserResponse>(result.Error.Message);
            }
            return Success(result.Value);
        }

        public async Task<Response<List<UserResponse>>> Handle(GetAllUsers request, CancellationToken cancellationToken)
        {
            var result = await _authServices.GetAllUsers();

            if (result.IsFailure)
            {
                return NotFound<List<UserResponse>>(result.Error.Message);
            }
            return Success(result.Value);
        }
    }
}
