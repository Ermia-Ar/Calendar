using Core.Application.ApplicationServices.Activities.Exceptions;
using Core.Application.ApplicationServices.Requests.Exceptions;
using Core.Application.Common;
using Core.Domain.Enum;
using Core.Domain.UnitOfWork;
using MediatR;

namespace Core.Application.ApplicationServices.Activities.Commands.RemoveMember;

public sealed class RemoveMemberOfActivityCommandHandler(
	IUnitOfWork unitOfWork,
	ICurrentUserServices currentUser)
		: IRequestHandler<RemoveMemberOfActivityCommandRequest>
{
	public readonly IUnitOfWork _unitOfWork = unitOfWork;
	public readonly ICurrentUserServices _currentUser = currentUser;

	public async Task Handle(RemoveMemberOfActivityCommandRequest request, CancellationToken cancellationToken)
	{
		var userId = _currentUser.GetUserId();

		var activity = await _unitOfWork.Activities
			.FindById(request.ActivityId, cancellationToken);

		if (activity.UserId != userId)
		{
			throw new OnlyActivityCreatorAllowedException();
		}

		//find request
		var userRequest = (await _unitOfWork.Requests.Find(null, request.ActivityId
		   , request.UserId, null, RequestStatus.Accepted, cancellationToken))
		   .FirstOrDefault();

		if (userRequest == null)
		{
			throw new NotFoundMemberException("No such member was found.");
		}

		var notification = await _unitOfWork.Notifications
			.Find(userRequest.Id, cancellationToken);

		_unitOfWork.Notifications.Remove(notification);

		_unitOfWork.Requests.Remove(userRequest);

		await _unitOfWork.SaveChangeAsync(cancellationToken);
	}
}
