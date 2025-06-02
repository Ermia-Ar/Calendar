using AutoMapper;
using Core.Application.Exceptions.User;
using Core.Application.Features.Auth.Commands;
using Core.Application.Services;
using Core.Domain;
using Core.Domain.Entity;
using MediatR;

namespace Core.Application.Features.Auth.Handler;

public class RegisterHandler
    : IRequestHandler<RegisterCommand, string>
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
    public async Task<string> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        //map to User
        var user = _mapper.Map<User>(request.RegisterRequest);
        user.Id = Guid.NewGuid().ToString();

        //Add to user table
        var result = await _unitOfWork.Users.AddUser(user, request.RegisterRequest.Password);
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
        return token;
    }

}


