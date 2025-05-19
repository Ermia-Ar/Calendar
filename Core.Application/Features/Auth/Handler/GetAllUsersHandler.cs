using AutoMapper;
using Core.Application.DTOs.UserDTOs;
using Core.Application.Features.Auth.Queries;
using Core.Application.Features.Exceptions;
using Core.Domain;
using Core.Domain.Shared;
using MediatR;

namespace Core.Application.Features.Auth.Handler
{

    public class GetAllUsersHandler : ResponseHandler
        , IRequestHandler<GetAllUsers, Response<List<UserResponse>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllUsersHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<List<UserResponse>>> Handle(GetAllUsers request, CancellationToken cancellationToken)
        {
            var users = await _unitOfWork.Users.GetAllUsers();
            var userResponse = _mapper.Map<List<UserResponse>>(users);

            return Success(userResponse);
        }

    }


}
