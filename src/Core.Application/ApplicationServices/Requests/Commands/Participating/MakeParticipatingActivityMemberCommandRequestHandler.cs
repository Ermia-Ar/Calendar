using Core.Application.ApplicationServices.Activities.Exceptions;
using Core.Application.ApplicationServices.Requests.Exceptions;
using Core.Application.Common;
using Core.Application.InternalServices.Auth.Dto;
using Core.Domain.Entities.Notifications;
using Core.Domain.UnitOfWork;
using Mapster;
using MediatR;

namespace Core.Application.ApplicationServices.Requests.Commands.Participating;

public sealed class MakeParticipatingActivityMemberCommandRequestHandler(
    ICurrentUserServices currentUserServices,
    IUnitOfWork unitOfWork) : IRequestHandler<MakeParticipatingActivityMemberCommandRequest>
{
    private readonly ICurrentUserServices _currentUserServices = currentUserServices;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    
    public async Task Handle(MakeParticipatingActivityMemberCommandRequest request, CancellationToken cancellationToken)
    {
        var user = (await _unitOfWork.Users
                .GetById(_currentUserServices.GetUserId(), cancellationToken))
                .Adapt<GetUserByIdDto>();
        
        //get request
        var activityRequest = await _unitOfWork.ActivityRequests
            .FindById(request.RequestId, cancellationToken);

        if (activityRequest is null)
            throw new InvalidRequestIdException();

        if (activityRequest.ReceiverId != user.Id)
            throw new NotFoundRequestException();
        
        var activityMember = await _unitOfWork.ActivityMembers
            .FindByActivityIdAndMemberId(user.Id, activityRequest.ActivityId, cancellationToken);
        
        // TODO : EXCEPTION
        if (activityMember is null)
            throw new Exception("Invalid activity member Id");
        
        var activity = await _unitOfWork.Activities
            .FindById(activityRequest.ActivityId, cancellationToken);
        
        if (activity is null)
            throw new InvalidActivityIdException();
                
        activityMember.MakeParticipating();
        
        //set default notification 
        var notificationBefore
            = user.DefaultNotificationBefore;

        var defaultNotification = activity.StartDate - notificationBefore;

        var notification = NotificationFactory
            .Create(user.Id, activity.Id, defaultNotification);

        _unitOfWork.Notifications.Add(notification);
        await _unitOfWork.SaveChangeAsync(cancellationToken);
    }
}