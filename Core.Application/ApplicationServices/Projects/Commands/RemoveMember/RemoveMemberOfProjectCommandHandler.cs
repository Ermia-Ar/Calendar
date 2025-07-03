using Core.Application.ApplicationServices.Projects.Exceptions;
using Core.Application.ApplicationServices.Requests.Exceptions;
using Core.Application.Common;
using Core.Domain.Entities.Notifications;
using Core.Domain.Enum;
using Core.Domain.UnitOfWork;
using MediatR;

namespace Core.Application.ApplicationServices.Projects.Commands.RemoveMember;

public sealed class RemoveMemberOfProjectCommandHandler(IUnitOfWork unitOfWork, ICurrentUserServices currentUserServices)
        : IRequestHandler<RemoveMemberOfProjectCommandRequest>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICurrentUserServices _currentUserServices = currentUserServices;

    public async Task Handle(RemoveMemberOfProjectCommandRequest request, CancellationToken cancellationToken)
    {
        //get current user id
        var userId = _currentUserServices.GetUserId();

        //check if user is creator of project or not
        var project = await _unitOfWork.Projects
            .FindById(request.ProjectId, cancellationToken);

        if (project.OwnerId != userId)
        {
            throw new OnlyProjectCreatorAllowedException();
        }
        // find requests for this projcet the userId is in RequestCommand
        var requests = (await _unitOfWork.Requests.Find(request.ProjectId, null
            , request.UserId, null, RequestStatus.Accepted, cancellationToken, false))
            .ToList();

        if (!requests.Any())
        {
            throw new NotFoundMemberException("No such member was found.");
        }

		//find all notifications for Project activities
		var notifications = new List<Notification>();
		foreach (var item in requests)
		{
			var notification = await _unitOfWork.Notifications
				   .Find(item.Id, cancellationToken);

			if (notification != null)
				notifications.Add(notification);
		}

		_unitOfWork.Notifications.RemoveRange(notifications);

		_unitOfWork.Requests.RemoveRange(requests);

        await _unitOfWork.SaveChangeAsync(cancellationToken);
    }
}
