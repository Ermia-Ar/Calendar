using Core.Domain.Interfaces;
using Mapster;
using MediatR;

namespace Core.Application.ApplicationServices.Activities.Queries.GetUserActivities;

public class GetUserActivitiesQueryHandler(IUnitOfWork unitOfWork, ICurrentUserServices currentUserServices)
            : IRequestHandler<GetUserActivitiesQueryRequest, List<GetUserActivitiesQueryResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICurrentUserServices _currentUserServices = currentUserServices;

    public async Task<List<GetUserActivitiesQueryResponse>> Handle(GetUserActivitiesQueryRequest request, CancellationToken cancellationToken)
    {
        var userId = _currentUserServices.GetUserId();

        var activities = await _unitOfWork.Requests.GetActivities
            (userId, request.UserIsOwner, cancellationToken
            , request.StartDate, request.Category, request.IsCompleted, request.IsHistory);

        var response = activities.Adapt<List<GetUserActivitiesQueryResponse>>();
        return response;
    }
}
