using Amazon.Runtime;
using Core.Application.ApplicationServices.Activities.Exceptions;
using Core.Application.Common;
using Core.Domain.UnitOfWork;
using Mapster;
using MediatR;
using SharedKernel.QueryFilterings;

namespace Core.Application.ApplicationServices.Activities.Queries.GetMembers;

public class GetMemberOfActivityQueryHandler(IUnitOfWork unitOfWork, ICurrentUserServices currentUser)
        : IRequestHandler<GetMemberOfActivityQueryRequest, PaginationResult<List<GetMemberOfActivityQueryResponse>>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICurrentUserServices _currentUser = currentUser;

    public async Task<PaginationResult<List<GetMemberOfActivityQueryResponse>>> Handle(GetMemberOfActivityQueryRequest request, CancellationToken cancellationToken)
    {
        //var userId = _currentUser.GetUserId();

        var members = await _unitOfWork.ActivityMembers
            .GetMemberOfActivity(request.ActivityId, request.Pagination,
                cancellationToken);
        
        var response = members.Responses.Adapt<List<GetMemberOfActivityQueryResponse>>();

        // if (!response.Any(x => x.Id == userId))
        //     throw new OnlyActivityMembersAllowedException();


        return new PaginationResult<List<GetMemberOfActivityQueryResponse>>(response, 
            request.Pagination.PageNumber, request.Pagination.PageSize, members.Count);
    }
}