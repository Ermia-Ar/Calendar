using Core.Application.ApplicationServices.Projects.Exceptions;
using Core.Application.Common;
using Core.Domain.UnitOfWork;
using Mapster;
using MediatR;
using SharedKernel.QueryFilterings;

namespace Core.Application.ApplicationServices.Projects.Queries.GetMembers;

public class GetMemberOfProjectQueryHandler(IUnitOfWork unitOfWork, ICurrentUserServices currentUserServices)
            : IRequestHandler<GetMemberOfProjectQueryRequest, PaginationResult<List<GetMemberOfProjectQueryResponse>>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICurrentUserServices _currentUserServices = currentUserServices;

    public async Task<PaginationResult<List<GetMemberOfProjectQueryResponse>>> Handle(GetMemberOfProjectQueryRequest request, CancellationToken cancellationToken)
    {
        var userId = _currentUserServices.GetUserId();

        //check if user is the owner of project or not 
        var members = await _unitOfWork.ProjectMembers
                .GetMemberOfProject(request.ProjectId,
                request.Pagination, cancellationToken);

        var response = members.Responses.Adapt<List<GetMemberOfProjectQueryResponse>>();

        if (!response.Any(x => x.MemberId == userId))
            throw new OnlyProjectMembersAllowedException();

        return new PaginationResult<List<GetMemberOfProjectQueryResponse>>(response,
            request.Pagination.PageNumber, request.Pagination.PageSize, members.Count);

    }
}
