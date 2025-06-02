using AutoMapper;
using Core.Application.DTOs.UserDTOs;
using Core.Application.Features.Auth.Queries;
using Core.Domain;
using MediatR;

namespace Core.Application.Features.Auth.Handler
{

    public class GetAllUsersHandler 
        : IRequestHandler<GetAllUsersQuery, List<UserResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllUsersHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<UserResponse>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _unitOfWork.Users.GetAllUsers(request.Search, request.Category, cancellationToken);
            var userResponse = _mapper.Map<List<UserResponse>>(users);

            return userResponse;
        }
    }
}
