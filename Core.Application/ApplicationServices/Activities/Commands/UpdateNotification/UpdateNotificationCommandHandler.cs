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


		notification.UpdateNotification(activity.StartDate - request.NotificationBefore);

		await _unitOfWork.SaveChangeAsync(cancellationToken);
	}
}
