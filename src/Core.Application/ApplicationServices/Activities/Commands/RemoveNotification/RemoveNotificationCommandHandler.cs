using Core.Application.Common;
using Core.Domain.UnitOfWork;
using MediatR;

namespace Core.Application.ApplicationServices.Activities.Commands.RemoveNotification;

public sealed class RemoveNotificationCommandHandler(
    IUnitOfWork unitOfWork,
    ICurrentUserServices currentUserServices
)
    : IRequestHandler<RemoveNotificationCommandRequest>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICurrentUserServices _currentUserServices = currentUserServices;
    
    public async Task Handle(RemoveNotificationCommandRequest request, CancellationToken cancellationToken)
    {
        var userId = _currentUserServices.GetUserId();
        
        var notification = await _unitOfWork.Notifications
            .FindByActivityIdAndUserId(userId, request.ActivityId, cancellationToken);
        
        if (notification is not null)
            _unitOfWork.Notifications.Remove(notification);
        
        
    }
}