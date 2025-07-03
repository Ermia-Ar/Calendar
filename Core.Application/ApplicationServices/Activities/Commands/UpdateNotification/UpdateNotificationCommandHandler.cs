using Core.Application.ApplicationServices.Activities.Exceptions;
using Core.Application.ApplicationServices.Requests.Exceptions;
using Core.Application.Common;
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

		var activity = await _unitOfWork.Activities
						.FindById(request.ActivityId, cancellationToken);

		var userRequest = (await _unitOfWork.Requests
						.Find(null, request.ActivityId, userId, null
						, Domain.Enum.RequestStatus.Accepted, cancellationToken))
						.FirstOrDefault();

		if (userRequest == null)
		{
			throw new NotFoundMemberException("not found member");
		}

		var notification = await _unitOfWork.Notifications
						.Find(userRequest.Id, cancellationToken);

		// if notification is null remove notificaiton for this user activity

		if (request.NotificationBefore == null)
		{
			userRequest.RemoveNotificaiton();	
		}
		else
		{
			var newNotificaitonDate = activity.StartDate - (request.NotificationBefore ?? TimeSpan.Zero);

			if (newNotificaitonDate <= DateTime.Now)
				throw new InvalidNotificationException();

			notification.UpdateNotification(newNotificaitonDate);
		}


		await _unitOfWork.SaveChangeAsync(cancellationToken);
	}
}
