using Core.Activities.ApplicationServices.Activities.Queries.GetComments;
using Core.Application.Common;
using Core.Domain.UnitOfWork;
using Mapster;
using MediatR;
using SharedKernel.QueryFilterings;

namespace Core.Application.ApplicationServices.Activities.Queries.GetComments;

public sealed class GetAllCommentQueryHandler(ICurrentUserServices currentUserServices, IUnitOfWork unitOfWork)
        : IRequestHandler<GetActivityCommentsQueryRequest, PaginationResult<List<GetActivityCommentsQueryResponse>>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICurrentUserServices _currentUserServices = currentUserServices;

    public async Task<PaginationResult<List<GetActivityCommentsQueryResponse>>> Handle(GetActivityCommentsQueryRequest request, CancellationToken cancellationToken)
    {
        Guid userId = _currentUserServices.GetUserId();

        var comments = await _unitOfWork.Comments.GetByActivityId(request.ActivityId,
            request.Filtering, request.Ordering,
            request.Pagination, cancellationToken);

        var response = comments.Responses.Adapt<List<GetActivityCommentsQueryResponse>>();

        return new PaginationResult<List<GetActivityCommentsQueryResponse>>(response, request.Pagination.PageNumber
            , request.Pagination.PageSize, comments.Count);
    }
}
