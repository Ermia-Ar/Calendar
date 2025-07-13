using Core.Application.ApplicationServices.Activities.Exceptions;
using Core.Application.ApplicationServices.Auth.Exceptions;
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
using Microsoft.Extensions.Configuration;

namespace Core.Application.ApplicationServices.Activities.Commands.AddSubActivity;

public sealed class AddSubActivityCommandHandler(
	IUnitOfWork unitOfWork,
	ICurrentUserServices currentUser,
	IConfiguration configuration)
	: IRequestHandler<AddSubActivityCommandRequest>
{
	private readonly IUnitOfWork _unitOfWork = unitOfWork;
	private readonly ICurrentUserServices _currentUser = currentUser;
	private readonly IConfiguration _configuration = configuration;

	public async Task Handle(AddSubActivityCommandRequest request, CancellationToken cancellationToken)
	{
		await using var transaction = await _unitOfWork.BeginTransaction(cancellationToken);

		try
		{
			var ownerId = _currentUser.GetUserId();

			//get base activity
			var baseActivity = await _unitOfWork.Activities
				.FindById(request.ActivityId, cancellationToken);

			if (baseActivity == null)
				throw new InvalidActivityIdException();

			if (baseActivity.UserId != ownerId)
				throw new OnlyActivityCreatorAllowedException();

			long? parentId = baseActivity.ParentId != null ? baseActivity.ParentId : baseActivity.Id;

			// create sub activity
			var subActivity = ActivityFactory.CreateSubActivity(parentId
				, ownerId, baseActivity.ProjectId
				, request.Title, request.Description
				, request.StartDate, request.Category
				, request.Duration);

			//add to table activity
			subActivity = _unitOfWork.Activities.Add(subActivity);

			await _unitOfWork.SaveChangeAsync(cancellationToken);

			//get memberIds of base project of activity
			var projectMemberIds = (await _unitOfWork.ProjectMembers
				.FindMemberIdsOfProject(subActivity.ProjectId, cancellationToken))
				.ToList();

			//send request for all memberIds
			var userRequests = new List<Request>();
			foreach (var receiverId in request.MemberIds)
			{
				//check
				var receiver = (await _unitOfWork
					.Users.GetById(receiverId, cancellationToken))
					.Adapt<GetUserByIdResponse>();

				if (receiver == null)
					throw new NotFoundUserIdException(receiverId);

				// check if the receiver is a member of base project
				var isGuest = projectMemberIds.Any(x => x != receiver.Id);

				var sendRequest = RequestFactory.Create
					(subActivity.Id, ownerId, receiver.Id,
						request.Message, isGuest);

				userRequests.Add(sendRequest);
			}

			//add requests table
			_unitOfWork.Requests.AddRange(userRequests);

			//add owner to activity members
			var activityOwner = ActivityMember
				.CreateOwner(ownerId, subActivity.Id);

			// add to activityMembers table
			activityOwner = _unitOfWork.
				ActivityMembers.Add(activityOwner);

			// set notification for owner
			// if NotificationBefore is null set default notification
			var notificationBefore = request.NotificationBefore ??
				TimeSpan.FromHours(NotificationSetting.DefaultNotificaiton);

			var notification = NotificationFactory
				.Create(activityOwner.Id
				, subActivity.StartDate - notificationBefore);

			activityOwner.SetNotification(notification.Id);

			//Add to Notifications table
			_unitOfWork.Notifications.Add(notification);

			await _unitOfWork.SaveChangeAsync(cancellationToken);

			await _unitOfWork.Commit(cancellationToken);
		}
		catch 
		{
			await _unitOfWork.Rollback(cancellationToken);
			throw;
		}

	}
}
