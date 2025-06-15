using AutoMapper;
using Core.Application.ApplicationServices.Auth.Exceptions;
using Core.Application.Services;
using Core.Domain.Entity.Users;
using Core.Domain.Interfaces;
using MediatR;

namespace Core.Application.ApplicationServices.Auth.Commands.Register;

public class RegisterCommandHandler
    : IRequestHandler<RegisterCommandRequest>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITokenServices _tokenServices;
    private readonly IMapper _mapper;

    public RegisterCommandHandler(IMapper mapper, IUnitOfWork unitOfWork, ITokenServices tokenServices)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _tokenServices = tokenServices;
    }
    public async Task Handle(RegisterCommandRequest request, CancellationToken cancellationToken)
    {
        //map to User
        var user = _mapper.Map<User>(request);
        user.Id = Guid.NewGuid().ToString();

        //Add to user table
        var result = await _unitOfWork.Users.AddUser(user, request.Password);
        if (result != null)
        {
            throw new BadRegisterRequestException(result);
        }
        result = await _unitOfWork.Users.AddRoleToUser(user, "User");
        if (result != null)
        {
            throw new BadRegisterRequestException(result);
        }
        var token = await _tokenServices.GetJWTToken(user);
    }
}


