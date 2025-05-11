using AutoMapper;
using Core.Application.DTOs.ActivityDTOs;
using Core.Application.Features.Activities.Queries;
using Core.Domain.Shared;
using Core.Domain;
using MediatR;

namespace Core.Application.Features.Activities.QueryHandlers
{
    public class GetCurrentActivityHandler : ResponseHandler
        , IRequestHandler<GettingActivitiesOwnedByTheUser, Response<List<ActivityResponse>>>
    {
        public IUnitOfWork _unitOfWork;
        public ICurrentUserServices _currentUser;
        public IMapper _mapper;

        public GetCurrentActivityHandler(IUnitOfWork unitOfWork, ICurrentUserServices currentUser, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
            _mapper = mapper;
        }

        public async Task<Response<List<ActivityResponse>>> Handle(GettingActivitiesOwnedByTheUser request, CancellationToken cancellationToken)
        {
            // get activities
            var userId = _currentUser.GetUserId();
            var activities = await _unitOfWork.Activities.GettingActivitiesOwnedByTheUser(userId, cancellationToken);
            // map to response
            var response = _mapper.Map<List<ActivityResponse>>(activities);

            return Success(response);
        }
    }
}