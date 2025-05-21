using AutoMapper;
using Core.Application.DTOs.ActivityDTOs;
using Core.Application.Features.Activities.Queries;
using Core.Domain;
using Core.Domain.Shared;
using MediatR;

namespace Core.Application.Features.Activities.QueryHandlers
{
    public class GetUserActivitiesHandler : ResponseHandler
        , IRequestHandler<GetUserActivitiesQuery, Response<List<ActivityResponse>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserServices _currentUserServices;
        private readonly IMapper _mapper;

        public GetUserActivitiesHandler(IUnitOfWork unitOfWork, ICurrentUserServices currentUserServices, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentUserServices = currentUserServices;
            _mapper = mapper;
        }

        public async Task<Response<List<ActivityResponse>>> Handle(GetUserActivitiesQuery request, CancellationToken cancellationToken)
        {
            var userName = _currentUserServices.GetUserName();
            var ownerId = _currentUserServices.GetUserId();

            var activities = await _unitOfWork.Requests.GetActivities
                (userName, request.UserIsOwner == true ? ownerId: null, cancellationToken
               , request.StartDate, request.Category, request.IsCompleted, request.IsHistory);

            var response = _mapper.Map<List<ActivityResponse>>(activities); ;
            return Success(response);
        }
    }
}
