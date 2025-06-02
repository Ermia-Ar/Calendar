using AutoMapper;
using Core.Application.DTOs.ActivityDTOs;
using Core.Application.Features.Activities.Queries;
using Core.Domain;
using MediatR;

namespace Core.Application.Features.Activities.QueryHandlers
{
    public class GetUserActivitiesHandler 
        : IRequestHandler<GetUserActivitiesQuery, List<ActivityResponse>>
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

        public async Task<List<ActivityResponse>> Handle(GetUserActivitiesQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUserServices.GetUserId();

            var activities = await _unitOfWork.Requests.GetActivities
                (userId, request.UserIsOwner, cancellationToken
                , request.StartDate, request.Category, request.IsCompleted, request.IsHistory);

            var response = _mapper.Map<List<ActivityResponse>>(activities); ;
            return response;
        }
    }
}
