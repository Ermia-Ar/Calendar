using Core.Application.Common;
using Core.Domain.UnitOfWork;
using Mapster;
using MediatR;
using Microsoft.Extensions.Configuration;
using SharedKernel.QueryFilterings;

namespace Core.Application.ApplicationServices.Projects.Queries.Activities;
public sealed class GetProjectActivitiesQueryHandler(IUnitOfWork unitOfWork, ICurrentUserServices currentUserServices, IConfiguration configuration)
            : IRequestHandler<GetProjectActivitiesQueryRequest, PaginationResult<List<GetProjectActivitiesQueryResponse>>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICurrentUserServices _currentUserServices = currentUserServices;
    private readonly IConfiguration _configuration = configuration;

    public async Task<PaginationResult<List<GetProjectActivitiesQueryResponse>>> Handle(GetProjectActivitiesQueryRequest request, CancellationToken cancellationToken)
    {
        long projectId = request.ProjectId;
        var userId = _currentUserServices.GetUserId();

        var activities = await _unitOfWork.ActivityMembers.GetActivitiesOfProjectForUserId
            (userId, projectId, request.Filtering
            , request.Ordering, request.Pagination,
            cancellationToken);


        var response = activities.Responses.Adapt<List<GetProjectActivitiesQueryResponse>>();
        return new PaginationResult<List<GetProjectActivitiesQueryResponse>>(response, request.Pagination.PageNumber
            , request.Pagination.PageSize, activities.Count);
    }
}








