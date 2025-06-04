using AutoMapper;
using Core.Domain;
using Mapster;
using MediatR;

namespace Core.Application.ApplicationServices.Auth.Queries.GetAllUsers
{

    public class GetAllUsersQueryHandler
        : IRequestHandler<GetAllUsersQueryRequest, List<GetAllUserQueryResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllUsersQueryHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<GetAllUserQueryResponse>> Handle(GetAllUsersQueryRequest request, CancellationToken cancellationToken)
        {
            var users = await _unitOfWork.Users.GetAllUsers(request.Search, request.Category, cancellationToken);
            var userResponse = users.Adapt<List<GetAllUserQueryResponse>>();

            return userResponse;
        }
    }
}
