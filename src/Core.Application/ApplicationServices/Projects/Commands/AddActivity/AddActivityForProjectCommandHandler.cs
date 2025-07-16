using Core.Application.ApplicationServices.Auth.Exceptions;
using Core.Application.ApplicationServices.Projects.Exceptions;
using Core.Application.Common;
using Core.Application.InternalServices.Auth.Dto;
using Core.Domain.Entities.Activities;
using Core.Domain.Entities.ActivityMembers;
using Core.Domain.Entities.Notifications;
using Core.Domain.Entities.Requests;
using Core.Domain.Helper;
using Core.Domain.UnitOfWork;
using Mapster;
using MediatR;

namespace Core.Application.ApplicationServices.Projects.Commands.AddActivity;

public sealed class AddActivityForProjectCommandHandler(
	IUnitOfWork unitOfWork,
	ICurrentUserServices currentUser
) : IRequestHandler<AddActivityForProjectCommandRequest>
{
	private readonly IUnitOfWork _unitOfWork = unitOfWork;
	private readonly ICurrentUserServices _currentUser = currentUser;

	public async Task Handle(AddActivityForProjectCommandRequest request, CancellationToken cancellationToken)
	{
		await using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
		try
		{
			var ownerId =_currentUser.GetUserId();

			//for check if user is the member of project or not 
			var memberIds = await _unitOfWork.ProjectMembers
				.FindMemberIdsOfProject(request.ProjectId, cancellationToken);

			if (!memberIds.Any(x => x == ownerId))
				throw new OnlyProjectMembersAllowedException();

			// create activity for project
			var activity = ActivityFactory.Create(ownerId, request.ProjectId
				, request.Title, request.Description
				, request.StartDate, request.Category
				, request.Duration);

			// add to activity table
			activity = _unitOfWork.Activities.Add(activity);
			await _unitOfWork.SaveChangeAsync(cancellationToken);

			var userRequests = new List<Request>();
			foreach (var receiverId in request.MemberIds)
			{
				//check
				var receiver = (await _unitOfWork
						.Users.GetById(receiverId, cancellationToken))
						.Adapt<GetUserByIdResponse>();

				if (receiver == null)
					throw new NotFoundUserIdException(receiverId);

				//check if receiver is member of base project
				var isGuest = memberIds
					.Any(x => x != receiverId);

				// create request for memberId
				var sendRequest = RequestFactory.Create(activity.Id, ownerId,
					receiverId, request.Message, isGuest);

				userRequests.Add(sendRequest);
			}

			//add requests table
			_unitOfWork.Requests.AddRange(userRequests);

			//
			//add owner to activity members
			var activityOwner = ActivityMember
				.CreateOwner(ownerId, activity.Id);

			activityOwner = _unitOfWork
				.ActivityMembers.Add(activityOwner);

			await _unitOfWork.SaveChangeAsync(cancellationToken);

			// create notification for owner
			// if NotificationBefore is null set default notification
			var notificationBefore = request.NotificationBefore ??
									 TimeSpan.FromHours(NotificationSetting.DefaultNotificaiton);

			var notification = NotificationFactory
				.Create(activityOwner.Id, activity.StartDate - notificationBefore);

			//add notifications table
			notification = _unitOfWork.Notifications.Add(notification);

			activityOwner.SetNotification(notification.Id);

			await _unitOfWork.SaveChangeAsync(cancellationToken);

			await transaction.CommitAsync(cancellationToken);
		}
		catch
		{
			await transaction.RollbackAsync(cancellationToken);
			throw;
		}
	}
}