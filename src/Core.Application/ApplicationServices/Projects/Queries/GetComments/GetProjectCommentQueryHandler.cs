using Core.Application.Common;
using Core.Domain.UnitOfWork;
using Mapster;
using MediatR;
using SharedKernel.QueryFilterings;

namespace Core.Application.ApplicationServices.Projects.Queries.GetComments;

public sealed class GetProjectCommentQueryHandler(
    ICurrentUserServices currentUserServices, 
    IUnitOfWork unitOfWork)
        : IRequestHandler<GetProjectCommentsQueryRequest, PaginationResult<List<GetProjectCommentsQueryResponse>>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICurrentUserServices _currentUserServices = currentUserServices;

    public async Task<PaginationResult<List<GetProjectCommentsQueryResponse>>> Handle(GetProjectCommentsQueryRequest request, CancellationToken cancellationToken)
    {
        Guid userId = _currentUserServices.GetUserId();

        var comments = await _unitOfWork.Comments.GetByProjectId(request.ProjectId,
            request.Filtering, request.Ordering,
            request.Pagination, cancellationToken);

        var response = comments.Responses.Adapt<List<GetProjectCommentsQueryResponse>>();

        return new PaginationResult<List<GetProjectCommentsQueryResponse>>(response, request.Pagination.PageNumber
            , request.Pagination.PageSize, comments.Count);
    }
}
