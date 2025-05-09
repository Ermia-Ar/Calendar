using AutoMapper;
using Core.Application.DTOs.UserDTOs;
using Core.Application.Features.Auth.Queries;
using Core.Domain;
using Core.Domain.Shared;
using MediatR;

namespace Core.Application.Features.Auth.Handler
{

    public class GetAllUsersHandler : ResponseHandler
        , IRequestHandler<GetAllUsers, Response<List<UserResponse>>>
    {
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;

        public GetAllUsersHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<List<UserResponse>>> Handle(GetAllUsers request, CancellationToken cancellationToken)
        {
            try
            {
                var users = await _unitOfWork.Users.GetAllUsers();
                var userResponse = _mapper.Map<List<UserResponse>>(users);

                return Success(userResponse);
            }
            catch
            {
                return BadRequest<List<UserResponse>>("something wrong !");
            }
        }

    }

    
}
