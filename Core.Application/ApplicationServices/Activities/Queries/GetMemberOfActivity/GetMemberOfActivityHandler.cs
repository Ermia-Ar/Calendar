using Core.Application.ApplicationServices.Activities.Exceptions;
using Core.Application.DTOs.UserDTOs;
using Core.Domain;
using Mapster;
using MediatR;

namespace Core.Application.ApplicationServices.Activities.Queries.GetMemberOfActivity;

public class GetMemberOfActivityHandler(IUnitOfWork unitOfWork, ICurrentUserServices currentUser)
        : IRequestHandler<GetMemberOfActivityQuery, List<UserResponse>>
{
    public readonly IUnitOfWork _unitOfWork = unitOfWork;
    public readonly ICurrentUserServices _currentUser = currentUser;

    public async Task<List<UserResponse>> Handle(GetMemberOfActivityQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUser.GetUserId();
        // check activity owner
        var activityMembers = await _unitOfWork.Requests
            .GetMemberOfActivity(request.ActivityId, cancellationToken);

        if (!activityMembers.Any(x => x.Id == userId))
        {
            throw new OnlyActivityMembersAllowedException();
        }
        var response = activityMembers.Adapt<List<UserResponse>>();

        return response;
    }
}