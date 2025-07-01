using AutoMapper;
using Core.Application.ApplicationServices.Auth.Exceptions;
using Core.Application.ExternalServices.Jwt;
using Core.Domain.Entities.Users;
using Core.Domain.UnitOfWork;
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
        var user = UserFactory.Create(request.UserName
            , request.Email, request.Password, request.Category);

        //Add to user table
        var result = await _unitOfWork.Users.AddUser(user, request.Password);
        if (result.Any())
        {
            throw new BadRegisterRequestException(result.First());
        }
        result = await _unitOfWork.Users.AddRoleToUser(user, "User");
        if (result.Any())
        {
            throw new BadRegisterRequestException(result.First());
        }
    }
}


