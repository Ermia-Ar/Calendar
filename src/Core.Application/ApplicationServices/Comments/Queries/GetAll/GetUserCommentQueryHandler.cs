using Core.Application.Common;
using Core.Application.Common.Exceptions;
using Core.Domain.UnitOfWork;
using Mapster;
using MediatR;
using SharedKernel.QueryFilterings;

namespace Core.Application.ApplicationServices.Comments.Queries.GetAll;

public sealed class GetUserCommentQueryHandler(ICurrentUserServices currentUserServices, IUnitOfWork unitOfWork)
        : IRequestHandler<GetUserCommentsQueryRequest, PaginationResult<List<GetUserCommentsQueryResponse>>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICurrentUserServices _currentUserServices = currentUserServices;

    public async Task<PaginationResult<List<GetUserCommentsQueryResponse>>> Handle(GetUserCommentsQueryRequest request, CancellationToken cancellationToken)
    {
        Guid userId = _currentUserServices.GetUserId();

        var comments = await _unitOfWork.Comments.GetByUserId(request.userId,
            request.Filtering, request.Ordering,
            request.Pagination, cancellationToken);

        var response = comments.Responses.Adapt<List<GetUserCommentsQueryResponse>>();

        return new PaginationResult<List<GetUserCommentsQueryResponse>>(response, request.Pagination.PageNumber
            , request.Pagination.PageSize, comments.Count);
    }
}
