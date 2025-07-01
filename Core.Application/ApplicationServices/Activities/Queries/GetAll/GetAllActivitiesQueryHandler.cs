using Core.Application.Common;
using Core.Domain.UnitOfWork;
using Mapster;
using MediatR;
using Microsoft.Extensions.Configuration;
using SharedKernel.QueryFilterings;

namespace Core.Application.ApplicationServices.Activities.Queries.GetAll;

public sealed class GetAllActivitiesQueryHandler(IUnitOfWork unitOfWork, ICurrentUserServices currentUserServices, IConfiguration configuration)
			: IRequestHandler<GetAllActivitiesQueryRequest, PaginationResult<List<GetAllActivitiesQueryResponse>>>
{
	private readonly IUnitOfWork _unitOfWork = unitOfWork;
	private readonly ICurrentUserServices _currentUserServices = currentUserServices;
	private readonly IConfiguration _configuration = configuration;

	public async Task<PaginationResult<List<GetAllActivitiesQueryResponse>>> Handle(GetAllActivitiesQueryRequest request, CancellationToken cancellationToken)
	{
		string projectId = _configuration["Public:ProjectId"];
		var userId = _currentUserServices.GetUserId();

		var activities = await _unitOfWork.Requests.GetAllActivities
			(userId, projectId, request.Filtering
			, request.Ordering, request.Pagination, cancellationToken);


		var response = activities.Responses.Adapt<List<GetAllActivitiesQueryResponse>>();
		return new PaginationResult<List<GetAllActivitiesQueryResponse>>(response, request.Pagination.PageNumber
			, request.Pagination.PageSize, activities.Count);
	}
}
