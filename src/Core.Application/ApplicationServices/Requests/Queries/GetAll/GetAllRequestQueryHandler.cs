using Core.Application.Common;
using Core.Domain.UnitOfWork;
using Mapster;
using MediatR;
using SharedKernel.QueryFilterings;

namespace Core.Application.ApplicationServices.Requests.Queries.GetAll;

public sealed class GetAllRequestQueryHandler(
	IUnitOfWork unitOfWork, 
	ICurrentUserServices currentUserServices)
			: IRequestHandler<GetAllRequestsQueryRequest, PaginationResult<List<GetAllRequestQueryResponse>>>
{
	private readonly IUnitOfWork _unitOfWork = unitOfWork;
	private readonly ICurrentUserServices _currentUserServices = currentUserServices;

	public async Task<PaginationResult<List<GetAllRequestQueryResponse>>> Handle(GetAllRequestsQueryRequest request, CancellationToken cancellationToken)
	{
		var userId = _currentUserServices.GetUserId();

		var requests = await _unitOfWork.Requests.GetAll
			(request.Filtering, request.Ordring
			, request.Pagination, cancellationToken);

		var response = requests.Responses.Adapt<List<GetAllRequestQueryResponse>>();
		return new PaginationResult<List<GetAllRequestQueryResponse>>(response, request.Pagination.PageNumber
			, request.Pagination.PageSize, requests.Count);
	}
}
