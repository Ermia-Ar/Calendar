using Core.Application.ApplicationServices.Activities.Exceptions;
using Core.Domain.Interfaces;
using Mapster;
using MediatR;

namespace Core.Application.ApplicationServices.Activities.Queries.GetMemberOfActivity;

public class GetMemberOfActivityQueryHandler(IUnitOfWork unitOfWork, ICurrentUserServices currentUser)
        : IRequestHandler<GetMemberOfActivityQueryRequest, List<GetMemberOfActivityQueryResponse>>
{
    public readonly IUnitOfWork _unitOfWork = unitOfWork;
    public readonly ICurrentUserServices _currentUser = currentUser;

    public async Task<List<GetMemberOfActivityQueryResponse>> Handle(GetMemberOfActivityQueryRequest request, CancellationToken cancellationToken)
    {
        var userId = _currentUser.GetUserId();
        // check activity owner
        var activityMembers = (await _unitOfWork.Requests
            .GetMemberOfActivity(request.ActivityId, cancellationToken))
            .Adapt<List<GetMemberOfActivityQueryResponse>>();

        if (!activityMembers.Any(x => x.Id == userId))
        {
            throw new OnlyActivityMembersAllowedException();
        }

        return activityMembers;
    }
}