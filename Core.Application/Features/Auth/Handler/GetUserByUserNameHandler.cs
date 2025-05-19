using AutoMapper;
using Core.Application.DTOs.UserDTOs;
using Core.Application.Features.Auth.Queries;
using Core.Application.Features.Exceptions;
using Core.Domain;
using Core.Domain.Shared;
using MediatR;

namespace Core.Application.Features.Auth.Handler
{
    public class GetUserByUserNameHandler : ResponseHandler
        , IRequestHandler<GetUserByUserNameQuery, Response<UserResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetUserByUserNameHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<UserResponse>> Handle(GetUserByUserNameQuery request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Users.FindByUserName(request.UserName);
            var userResponse = _mapper.Map<UserResponse>(user);
            return Success(userResponse);
        }
    }


}
