using Core.Application.ApplicationServices.Requests.Exceptions;
using Core.Application.Common;
using Core.Domain.Entities.Notifications;
using Core.Domain.Enum;
using Core.Domain.UnitOfWork;
using MediatR;

namespace Core.Application.ApplicationServices.Projects.Commands.Exiting;

public sealed class ExitingProjectCommandHandler(IUnitOfWork unitOfWork, ICurrentUserServices currentUserServices)
		: IRequestHandler<ExitingProjectCommandRequest>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICurrentUserServices _currentUserServices = currentUserServices;

	public async Task Handle(ExitingProjectCommandRequest request, CancellationToken cancellationToken)
    {
        var userId = _currentUserServices.GetUserId();

        //get all requests for this project and its activities
        var requests = (await _unitOfWork.Requests.Find(request.ProjectId, null
             , userId, null, RequestStatus.Accepted, cancellationToken, false))
             .ToList();

        if (!requests.Any())
        {
            throw new NotFoundMemberException("You are not a member of this project.");
        }

        //find all notifications for Project activities
        var notifications = new List<Notification>();
        foreach (var item in requests)
        {
			var notification = await _unitOfWork.Notifications
				   .Find(item.Id, cancellationToken);

            if(notification != null)
                notifications.Add(notification);
		}

		_unitOfWork.Notifications.RemoveRange(notifications);
		_unitOfWork.Requests.RemoveRange(requests);
        await _unitOfWork.SaveChangeAsync(cancellationToken);

    }
}
