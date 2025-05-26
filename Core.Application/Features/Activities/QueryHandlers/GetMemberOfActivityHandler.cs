using AutoMapper;
using Core.Application.Features.Activities.Queries;
using Core.Domain.Shared;
using Core.Domain;
using MediatR;
using Core.Application.DTOs.UserDTOs;
using Core.Application.Features.Exceptions;

namespace Core.Application.Features.Activities.QueryHandlers
{
    public class GetMemberOfActivityHandler : ResponseHandler
        , IRequestHandler<GetMemberOfActivityQuery, Response<List<UserResponse>>>
    {
        public readonly IUnitOfWork _unitOfWork;
        public readonly ICurrentUserServices _currentUser;
        public readonly IMapper _mapper;

        public GetMemberOfActivityHandler(IUnitOfWork unitOfWork, ICurrentUserServices currentUser, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
            _mapper = mapper;
        }

        public async Task<Response<List<UserResponse>>> Handle(GetMemberOfActivityQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUser.GetUserId();
            // check activity owner
            var activityMembers = await _unitOfWork.Requests
                .GetMemberOfActivity(request.ActivityId, cancellationToken);

            if (!activityMembers.Any(x => x.Id == userId))
            {
                throw new NotFoundException("Only the members of this activity has access to this section.");
            }
            var response = _mapper.Map<List<UserResponse>>(activityMembers); 

            return Success(response);
        }
    }
}