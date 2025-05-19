using AutoMapper;
using Core.Application.DTOs.ActivityDTOs;
using Core.Application.Features.Activities.Queries;
using Core.Domain;
using Core.Domain.Shared;
using MediatR;

namespace Core.Application.Features.Activities.QueryHandlers
{
    public class GetAllUserActivitiesHandler : ResponseHandler
        , IRequestHandler<GetAllUserActivitiesQuery, Response<List<ActivityResponse>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserServices _currentUserServices;
        private readonly IMapper _mapper;

        public GetAllUserActivitiesHandler(IUnitOfWork unitOfWork, ICurrentUserServices currentUserServices, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentUserServices = currentUserServices;
            _mapper = mapper;
        }

        public async Task<Response<List<ActivityResponse>>> Handle(GetAllUserActivitiesQuery request, CancellationToken cancellationToken)
        {
            var userName = _currentUserServices.GetUserName();
            var userId = _currentUserServices.GetUserId();

            var ownedActivities = await _unitOfWork.Activities
                .GettingActivitiesOwnedByTheUser(userId, cancellationToken, 
                request.StartDate , request.Category, request.IsCompleted , request.IsHistory);

            if (!request.UserIsOwner)
            {
                ownedActivities.AddRange(await _unitOfWork.Requests
                    .GetActivitiesThatTheUserIsMemberOf(userName, cancellationToken
                , request.StartDate, request.Category, request.IsCompleted , request.IsHistory));
            }


            var response = _mapper.Map<List<ActivityResponse>>(ownedActivities); ;
            return Success(response);
        }
    }
}
