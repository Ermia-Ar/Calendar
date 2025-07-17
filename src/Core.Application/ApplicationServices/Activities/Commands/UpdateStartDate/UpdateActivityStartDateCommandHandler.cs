using Core.Application.ApplicationServices.Activities.Exceptions;
using Core.Application.Common;
using Core.Domain.UnitOfWork;
using MediatR;

namespace Core.Application.ApplicationServices.Activities.Commands.UpdateStartDate;

public sealed class UpdateActivityStartDateCommandHandler(ICurrentUserServices currentUserServices, IUnitOfWork unitOfWork)
		: IRequestHandler<UpdateActivityStartDateCommandRequest>
{
	private readonly ICurrentUserServices _currentUserServices = currentUserServices;
	private readonly IUnitOfWork _unitOfWork = unitOfWork;
	
	public async Task Handle(UpdateActivityStartDateCommandRequest request, CancellationToken cancellationToken)
	{
		var userId = _currentUserServices.GetUserId();

		var activity = await _unitOfWork.Activities
			.FindById(request.activityId, cancellationToken);

		if (activity == null)
			throw new InvalidActivityIdException();
		
		if (activity.OwnerId != userId)
			throw new OnlyActivityCreatorAllowedException();

		//sync all notifications with new startDate
		var notifications = await _unitOfWork.Notifications
			.FindByActivityId(request.activityId, cancellationToken);

        foreach (var notification in notifications)
        {
			var notificationBefore = activity.StartDate - notification.NotificationDate;
			var newNotificationDate = request.NewStartDate - notificationBefore;

			notification.UpdateNotification(newNotificationDate);
        }

		activity.ChangeStartDate(request.NewStartDate);

		await _unitOfWork.SaveChangeAsync(cancellationToken);
	}
}