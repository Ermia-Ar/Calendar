using Core.Application.ApplicationServices.Activities.Queries.GetAll;
using Core.Application.Common;
using Core.Domain.UnitOfWork;
using Mapster;
using MediatR;
using SharedKernel.QueryFilterings;
using System.Collections.Immutable;

namespace Core.Application.ApplicationServices.Projects.Queries.GetAll;

public sealed class GetAllProjectsQueryHandler(IUnitOfWork unitOfWork, ICurrentUserServices currentUserServices)
            : IRequestHandler<GetAllProjectsQueryRequest, PaginationResult<List<GetAllProjectQueryResponse>>>
{
    private readonly ICurrentUserServices _currentUserServices = currentUserServices;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<PaginationResult<List<GetAllProjectQueryResponse>>> Handle(GetAllProjectsQueryRequest request, CancellationToken cancellationToken)
    {
        var userId = _currentUserServices.GetUserId();

        var projects = await _unitOfWork.ProjectMembers
            .GetProjectOfUserId(userId, request.Filtering, request.Ordring,
            request.Pagination, cancellationToken);

        var response = projects.Responses.Adapt<List<GetAllProjectQueryResponse>>();

        return new PaginationResult<List<GetAllProjectQueryResponse>>(response, request.Pagination.PageNumber
            , request.Pagination.PageSize, projects.Count);
    }
}
