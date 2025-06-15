using Core.Domain.Interfaces;
using Mapster;
using MediatR;

namespace Core.Application.ApplicationServices.Activities.Queries.GetAll;

public sealed class GetAllActivitiesQueryHandler(IUnitOfWork unitOfWork, ICurrentUserServices currentUserServices)
            : IRequestHandler<GetAllActivitiesQueryRequest, List<GetAllActivitiesQueryResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICurrentUserServices _currentUserServices = currentUserServices;

    public async Task<List<GetAllActivitiesQueryResponse>> Handle(GetAllActivitiesQueryRequest request, CancellationToken cancellationToken)
    {
        var userId = _currentUserServices.GetUserId();

        var activities = await _unitOfWork.Requests.GetActivities
            (userId, request.Filtering.UserIsOwner, cancellationToken
            , request.Filtering.StartDate, request.Filtering.Category
            , request.Filtering.IsCompleted, request.Filtering.IsHistory);

        var response = activities.Adapt<List<GetAllActivitiesQueryResponse>>();
        return response;
    }
}
