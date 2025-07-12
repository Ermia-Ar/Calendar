using Core.Application.Common;
using Core.Application.Common.Exceptions;
using Core.Domain.UnitOfWork;
using Mapster;
using MediatR;
using SharedKernel.QueryFilterings;

namespace Core.Application.ApplicationServices.Comments.Queries.GetAll;

public sealed class GetAllCommentQueryHandler(ICurrentUserServices currentUserServices, IUnitOfWork unitOfWork)
        : IRequestHandler<GetAllCommentsQueryRequest, PaginationResult<List<GetAllCommentsQueryResponse>>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICurrentUserServices _currentUserServices = currentUserServices;

    public async Task<PaginationResult<List<GetAllCommentsQueryResponse>>> Handle(GetAllCommentsQueryRequest request, CancellationToken cancellationToken)
    {
        Guid userId = _currentUserServices.GetUserId();

        if (request.Filtering.ProjectId == null
            && request.Filtering.ActivityId == null
            && request.Filtering.userId == null)
        {
            throw new BadRequestExceptions("bad request");
        }

        var comments = await _unitOfWork.Comments.GetAll(request.Filtering, request.Ordering,
            request.Pagination, cancellationToken);

        var response = comments.Responses.Adapt<List<GetAllCommentsQueryResponse>>();

        return new PaginationResult<List<GetAllCommentsQueryResponse>>(response, request.Pagination.PageNumber
            , request.Pagination.PageSize, comments.Count);
    }
}
