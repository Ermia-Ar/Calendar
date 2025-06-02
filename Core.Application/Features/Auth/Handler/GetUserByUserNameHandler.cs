using AutoMapper;
using Core.Application.DTOs.UserDTOs;
using Core.Application.Features.Auth.Queries;
using Core.Domain;
using MediatR;

namespace Core.Application.Features.Auth.Handler
{
    public class GetUserByUserNameHandler 
        : IRequestHandler<GetUserByUserNameQuery, UserResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetUserByUserNameHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<UserResponse> Handle(GetUserByUserNameQuery request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Users.FindByUserName(request.UserName);
            var userResponse = _mapper.Map<UserResponse>(user);
            return userResponse;
        }
    }


}
