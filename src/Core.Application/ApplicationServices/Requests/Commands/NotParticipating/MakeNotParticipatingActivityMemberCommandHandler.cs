using Core.Application.ApplicationServices.Activities.Exceptions;
using Core.Application.ApplicationServices.Requests.Exceptions;
using Core.Application.Common;
using Core.Domain.UnitOfWork;
using MediatR;

namespace Core.Application.ApplicationServices.Requests.Commands.NotParticipating;

public sealed class MakeNotParticipatingActivityMemberCommandHandler(
    IUnitOfWork unitOfWork,
    ICurrentUserServices currentUserServices
)
    : IRequestHandler<MakeNotParticipatingActivityMemberCommandRequest>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICurrentUserServices _currentUserServices = currentUserServices;
    
    public async Task Handle(MakeNotParticipatingActivityMemberCommandRequest request, CancellationToken cancellationToken)
    {
        var userId = _currentUserServices.GetUserId();
        //get request
        var activityRequest = await _unitOfWork.ActivityRequests
            .FindById(request.RequestId, cancellationToken);

        if (activityRequest is null)
            throw new InvalidRequestIdException();

        if (activityRequest.ReceiverId != userId)
            throw new NotFoundRequestException();
        
        var activityMember = await _unitOfWork.ActivityMembers
            .FindByActivityIdAndMemberId(userId, activityRequest.ActivityId, cancellationToken);
        
        // TODO : MADE EXCEPTION
        if (activityMember is null)
            throw new Exception("Invalid activity member Id");
        
        var activity = await _unitOfWork.Activities
            .FindById(activityRequest.ActivityId, cancellationToken);

        if (activity is null)
            throw new InvalidActivityIdException();
        
        activityMember.MakeNotParticipating(request.Reason);
        
        var notification = await _unitOfWork.Notifications
            .FindByActivityIdAndUserId(userId, activityMember.ActivityId, cancellationToken);

        await _unitOfWork.SaveChangeAsync(cancellationToken);
    }
}