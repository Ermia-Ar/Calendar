using AutoMapper;
using Core.Application.Features.Auth.Commands;
using Core.Domain;
using Core.Domain.Entity;
using Core.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Core.Application.Features.Auth.Handler
{
    public class RegisterHandler : ResponseHandler
        , IRequestHandler<RegisterCommand, Response<string>>
    {
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;

        public RegisterHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public async Task<Response<string>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            //map to User
            var user = _mapper.Map<User>(request);

            //Add to user table
            var result = await _unitOfWork.Users.AddUser(user, request.RegisterRequest.Password);
            if (!result.Succeeded)
            {
                return BadRequest<string>(result.Errors.First().Description);
            }
            result = await _unitOfWork.Users.AddRoleToUser(user, "User");
            if (!result.Succeeded)
            {
                return BadRequest<string>(result.Errors.First().Description);
            }

            return NoContent<string>();
        }

    }

    
}
