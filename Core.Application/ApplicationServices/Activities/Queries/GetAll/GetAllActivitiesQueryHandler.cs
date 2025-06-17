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
        string projectId = "8c56ac14-ae28-4425-9a19-690d27d3a16d";
        var userId = _currentUserServices.GetUserId();

        var activities = await _unitOfWork.Requests.GetActivities
            (userId, projectId, cancellationToken
            , request.Filtering.StartDate, request.Filtering.Category
            , request.Filtering.IsCompleted, request.Filtering.IsHistory);

        var response = activities.Adapt<List<GetAllActivitiesQueryResponse>>();
        return response;
    }
}
