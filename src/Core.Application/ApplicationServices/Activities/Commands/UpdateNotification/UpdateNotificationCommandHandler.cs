using Core.Application.ApplicationServices.Activities.Exceptions;
using Core.Application.ApplicationServices.Requests.Exceptions;
using Core.Application.Common;
using Core.Domain.Entities.Notifications;
using Core.Domain.UnitOfWork;
using MediatR;

namespace Core.Application.ApplicationServices.Activities.Commands.UpdateNotification;

public sealed class UpdateNotificationCommandHandler(ICurrentUserServices currentUserServices, IUnitOfWork unitOfWork)
		: IRequestHandler<UpdateNotificationCommandRequest>
{
	private readonly IUnitOfWork _unitOfWork = unitOfWork;
	private readonly ICurrentUserServices _currentUserServices = currentUserServices;

	public async Task Handle(UpdateNotificationCommandRequest request, CancellationToken cancellationToken)
	{
		var userId = _currentUserServices.GetUserId();
		// get Members
		var activityMember = await _unitOfWork.ActivityMembers
			.FindByActivityIdAndMemberId(userId, request.ActivityId,
				cancellationToken);

		if (activityMember == null)
			throw new NotFoundMemberException("not found member");

		var notification = await _unitOfWork.Notifications
			.FindByActivityIdAndUserId(userId, activityMember.ActivityId, cancellationToken);
		
		var activity = await _unitOfWork.Activities
			.FindById(request.ActivityId, cancellationToken);
			
		if (activity is null)
			throw new InvalidActivityIdException();
		
		// if notification is null remove notification for this user activity
		if (notification != null)
		{
			var newNotification = activity.StartDate - request.NotificationBefore;
		
			if (newNotification <= DateTime.UtcNow)
				throw new InvalidNotificationException();

			notification.UpdateNotification(newNotification);
		}
		

		await _unitOfWork.SaveChangeAsync(cancellationToken);
	}
}
