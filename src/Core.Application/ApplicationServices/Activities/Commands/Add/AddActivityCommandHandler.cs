using Amazon.Runtime.Internal.Auth;
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
using Newtonsoft.Json.Serialization;

namespace Core.Application.ApplicationServices.Activities.Commands.Add;

public sealed class AddActivityCommandHandler(
	IUnitOfWork unitOfWork,
	ICurrentUserServices currentUser,
	IConfiguration configuration
	): IRequestHandler<AddActivityCommandRequest>
{
	private readonly IUnitOfWork _unitOfWork = unitOfWork;
	private readonly ICurrentUserServices _currentUser = currentUser;
	private readonly IConfiguration _configuration = configuration;

	public async Task Handle(AddActivityCommandRequest request, CancellationToken cancellationToken)
	{
		await using var transaction = await _unitOfWork.BeginTransaction(cancellationToken);

		try
		{
			var ownerId = _currentUser.GetUserId();
			var defaultProjectId = long.Parse(configuration["PUBLIC:PROJECTID"]);

			// create new activity
			var activity = ActivityFactory.Create(ownerId, defaultProjectId
				, request.Title, request.Description
				, request.StartDate, request.Category
				, request.Duration);

			//add to activity table
			activity = _unitOfWork.Activities.Add(activity);
			await _unitOfWork.SaveChangeAsync(cancellationToken);

			//create request for all membersIds
			var userRequests = new List<Request>();
			foreach (var receiverId in request.MemberIds)
			{
				////check
				var receiver = (await _unitOfWork
					.Users.GetById(receiverId, cancellationToken))
					.Adapt<GetUserByIdResponse>();

				if (receiver == null)
					throw new NotFoundUserIdException(receiverId);

				//create request for memberId
				var sendRequest = RequestFactory.Create(activity.Id
					, ownerId, receiverId, request.Message, false);

				userRequests.Add(sendRequest);
			}

			//add requests table
			_unitOfWork.Requests.AddRange(userRequests);

			//add owner to activity members
			var activityOwner = ActivityMember
				.CreateOwner(ownerId, activity.Id);

			// add to activityMembers table
			activityOwner = _unitOfWork.
				ActivityMembers.Add(activityOwner);

			await _unitOfWork.SaveChangeAsync(cancellationToken);


			// set notification for owner
			// if NotificationBefore is null set default notification
			var notificationBefore = request.NotificationBefore ??
				TimeSpan.FromHours(NotificationSetting.DefaultNotificaiton);

			var notification = NotificationFactory
				.Create(activityOwner.Id
				, activity.StartDate - notificationBefore);

			//Add notifications table
			notification = _unitOfWork.Notifications.Add(notification);

			//set notification
			activityOwner.SetNotification(notification.Id);

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