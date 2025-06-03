using AutoMapper;
using Core.Application.ApplicationServices.Activities.Queries.GetById;
using Core.Domain;
using Mapster;
using MediatR;

namespace Core.Application.ApplicationServices.Activities.Queries.GetUserActivities
{
    public class GetUserActivitiesHandler(IUnitOfWork unitOfWork, ICurrentUserServices currentUserServices)
                : IRequestHandler<GetUserActivitiesQuery, List<GetByIdActivityQueryResponse>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ICurrentUserServices _currentUserServices = currentUserServices;

        public async Task<List<GetByIdActivityQueryResponse>> Handle(GetUserActivitiesQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUserServices.GetUserId();

            var activities = await _unitOfWork.Requests.GetActivities
                (userId, request.UserIsOwner, cancellationToken
                , request.StartDate, request.Category, request.IsCompleted, request.IsHistory);

            var response = activities.Adapt<List<GetByIdActivityQueryResponse>>();
            return response;
        }
    }
}
