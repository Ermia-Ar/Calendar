using AutoMapper;
using Core.Application.Features.Auth.Commands;
using Core.Application.Features.Exceptions;
using Core.Domain;
using Core.Domain.Entity;
using Core.Domain.Helper;
using Core.Domain.Interfaces;
using Core.Domain.Shared;
using MediatR;

namespace Core.Application.Features.Auth.Handler
{
    public class RegisterHandler : ResponseHandler
        , IRequestHandler<RegisterCommand, Response<JwtAuthResult>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenServices _tokenServices;
        private readonly IMapper _mapper;

        public RegisterHandler(IMapper mapper, IUnitOfWork unitOfWork, ITokenServices tokenServices)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _tokenServices = tokenServices;
        }
        public async Task<Response<JwtAuthResult>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            //map to User
            var user = _mapper.Map<User>(request.RegisterRequest);

            //Add to user table
            var result = await _unitOfWork.Users.AddUser(user, request.RegisterRequest.Password);
            if (!result.Succeeded)
            {
                throw new BadRequestException(result.Errors.First().Description);
            }
            result = await _unitOfWork.Users.AddRoleToUser(user, "User");
            if (!result.Succeeded)
            {
                throw new BadRequestException(result.Errors.First().Description);
            }
            var token = await _tokenServices.GetJWTToken(user);
            return Created(token);
        }

    }

    
}
