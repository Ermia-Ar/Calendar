using Core.Application.ApplicationServices.Activities.Exceptions;
using Core.Application.ApplicationServices.Requests.Exceptions;
using Core.Application.Common;
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

		if (activity == null)
			throw new InvalidActivityIdException();

		if (activity.UserId != userId)
			throw new OnlyActivityCreatorAllowedException();

		//find activityMembe
		var activityMember = await _unitOfWork.ActivityMembers
			.FindByActivityIdAndMemberId(request.UserId,
			request.ActivityId, cancellationToken);

		if (activityMember == null)
			throw new NotFoundMemberException("No such member was found.");

		var notification = await _unitOfWork.Notifications
			.Find(activityMember.Id, cancellationToken);

		_unitOfWork.Notifications.Remove(notification);

		_unitOfWork.ActivityMembers.Remove(activityMember);

		await _unitOfWork.SaveChangeAsync(cancellationToken);
	}
}
