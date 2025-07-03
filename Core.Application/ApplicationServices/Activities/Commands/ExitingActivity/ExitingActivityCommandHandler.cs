using Core.Application.ApplicationServices.Requests.Exceptions;
using Core.Application.Common;
using Core.Domain.Enum;
using Core.Domain.UnitOfWork;
using MediatR;

namespace Core.Application.ApplicationServices.Activities.Commands.ExitingActivity;

public sealed class ExitingActivityCommandHandler(IUnitOfWork unitOfWork, ICurrentUserServices currentUser)
        : IRequestHandler<ExitingActivityCommandRequest>
{
    public readonly IUnitOfWork _unitOfWork = unitOfWork;
    public readonly ICurrentUserServices _currentUser = currentUser;

    public async Task Handle(ExitingActivityCommandRequest request, CancellationToken cancellationToken)
    {
        var userId = _currentUser.GetUserId();

        var userRequest = (await _unitOfWork.Requests.Find(null, request.ActivityId
            , userId, null, RequestStatus.Accepted, cancellationToken))
            .FirstOrDefault();

        if (userRequest == null)
        {
            throw new NotFoundMemberException("You are not a member of this activity.");
        }

		var notification = await _unitOfWork.Notifications
	       .Find(userRequest.Id, cancellationToken);

		_unitOfWork.Notifications.Remove(notification);

		_unitOfWork.Requests.Remove(userRequest);

        await _unitOfWork.SaveChangeAsync(cancellationToken);
    }
}
