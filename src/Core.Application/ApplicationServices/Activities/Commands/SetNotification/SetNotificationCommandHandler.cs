using Core.Application.ApplicationServices.Activities.Exceptions;
using Core.Application.ApplicationServices.Requests.Exceptions;
using Core.Application.Common;
using Core.Domain.Entities.Notifications;
using Core.Domain.UnitOfWork;
using MediatR;

namespace Core.Application.ApplicationServices.Activities.Commands.SetNotification;

public class SetNotificationCommandHandler(
    IUnitOfWork unitOfWork,
    ICurrentUserServices currentUserServices
)
    : IRequestHandler<SetNotificationCommandRequest>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICurrentUserServices _currentUserServices = currentUserServices;
    
    public async Task Handle(SetNotificationCommandRequest request, CancellationToken cancellationToken)
    {
        var userId = _currentUserServices.GetUserId();
        
        var activity = await _unitOfWork.Activities
            .FindById(request.ActivityId, cancellationToken);
        
        if (activity is null)
            throw new InvalidActivityIdException();
        
        var isMemberOfActivity = await _unitOfWork.ActivityMembers
            .IsMemberOfActivity(userId, request.ActivityId, cancellationToken);
            
        if (!isMemberOfActivity)
            throw new NotFoundMemberException("Not found member of activity");
        
        var notificationDate = activity.StartDate - request.NotificationBefore;

        var notification = NotificationFactory
            .Create(userId, request.ActivityId, notificationDate);

        await _unitOfWork.SaveChangeAsync(cancellationToken);
    }
}