using AutoMapper;
using Core.Application.DTOs.ActivityDTOs;
using Core.Application.Features.Activities.Queries;
using Core.Domain.Shared;
using Core.Domain;
using MediatR;

namespace Core.Application.Features.Activities.QueryHandlers
{

    public class GetHistoryOfActivitiesHandler : ResponseHandler
        , IRequestHandler<GetHistoryOfActivitiesQuery, Response<List<ActivityResponse>>>
    {
        public readonly IUnitOfWork _unitOfWork;
        public readonly ICurrentUserServices _currentUser;
        public readonly IMapper _mapper;

        public GetHistoryOfActivitiesHandler(IUnitOfWork unitOfWork, ICurrentUserServices currentUser, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
            _mapper = mapper;
        }
        public async Task<Response<List<ActivityResponse>>> Handle(GetHistoryOfActivitiesQuery request, CancellationToken cancellationToken)
        {
            // get activities
            var userId = _currentUser.GetUserId();
            var activities = await _unitOfWork.Activities.GetHistoryOfUserActivities(userId, cancellationToken);
            // map to response
            var response = _mapper.Map<List<ActivityResponse>>(activities);

            return Success(response);
        }

    }
}