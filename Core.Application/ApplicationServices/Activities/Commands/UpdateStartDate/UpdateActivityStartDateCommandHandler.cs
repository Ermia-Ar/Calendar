using Core.Application.ApplicationServices.Activities.Exceptions;
using Core.Application.Common;
using Core.Domain.Enum;
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

		var activity = await _unitOfWork.Activities.FindById(request.activityId, cancellationToken);
		if (activity.UserId != userId)
		{
			throw new OnlyActivityCreatorAllowedException();
		}

		//sync all notifications with new startDate
		var members = await _unitOfWork.Requests
			.Find(null, request.activityId, null
			, null, RequestStatus.Accepted, cancellationToken);

        foreach (var member in members)
        {
			var notification = await _unitOfWork.Notifications
									.Find(member.Id, cancellationToken);

			var notificationBefor = activity.StartDate - notification.NotificationDate;
			var newNotificationDate = request.NewStartDate - notificationBefor;

			notification.UpdateNotification(newNotificationDate);
        }


		activity.ChangeStartDate(request.NewStartDate);

		await _unitOfWork.SaveChangeAsync();
	}
}