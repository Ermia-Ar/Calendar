using Core.Application.ApplicationServices.Auth.Exceptions;
using Core.Application.Common;
using Core.Application.InternalServices.Auth.Dto;
using Core.Domain.Entities.Activities;
using Core.Domain.Entities.ActivityMembers;
using Core.Domain.Entities.Notifications;
using Core.Domain.Entities.Requests;
using Core.Domain.Enum;
using Core.Domain.UnitOfWork;
using Mapster;
using MediatR;
using Microsoft.Extensions.Configuration;


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
		await using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

		try
		{
			var owner = (await _unitOfWork.Users
					.GetById(_currentUser.GetUserId(), cancellationToken))
					.Adapt<GetUserByIdDto>();
			
			var defaultProjectId = long.Parse(_configuration["PUBLIC:PROJECTID"]);

			// create new activity
			var activity = ActivityFactory.Create(owner.Id, defaultProjectId
				, request.Title, request.Description
				, request.StartDate, request.Type
				, request.Duration);

			//add to activity table
			activity = _unitOfWork.Activities.Add(activity);
			await _unitOfWork.SaveChangeAsync(cancellationToken);

			//create request for all membersIds
			var activityRequests = new List<ActivityRequest>();
			
			var activityMembers = new List<ActivityMember>();
			foreach (var receiverId in request.MemberIds)
			{
				//check
				var receiver = (await _unitOfWork
					.Users.GetById(receiverId, cancellationToken))
					.Adapt<GetUserByIdDto>();

				if (receiver == null)
					throw new NotFoundUserIdException(receiverId);

				//create request for memberId
				var sendRequest = ActivityRequestFactory
					.Create(receiverId, activity.Id, "please join");
				activityRequests.Add(sendRequest);
				
				//create activityMember for memberId
				var activityMember = ActivityMember
					.Create(receiverId, activity.Id, 
						false, ParticipationStatus.Pending);
				activityMembers.Add(activityMember);
			}
			//add to activityMembers table
			_unitOfWork.ActivityMembers.AddRange(activityMembers);
			//add to ActivityRequests table
			_unitOfWork.ActivityRequests.AddRange(activityRequests);

			//add owner to activity members
			var activityOwner = ActivityMember
				.Create(owner.Id, activity.Id, 
					false,  ParticipationStatus.Participating);

			// add to activityMembers table
			_unitOfWork.ActivityMembers.Add(activityOwner);
				
			// set notification for owner
			// if NotificationBefore is null set default notification
			var notificationBefore = request.NotificationBefore ??
							owner.DefaultNotificationBefore;

			var notification = NotificationFactory.Create(owner.Id, activity.Id,
				activity.StartDate - notificationBefore);

			//Add to notifications table
			_unitOfWork.Notifications.Add(notification);

			await _unitOfWork.SaveChangeAsync(cancellationToken);

			await _unitOfWork.CommitTransactionAsync(cancellationToken);
		}
		catch 
		{
			await _unitOfWork.RoleBackTransactionAsync(cancellationToken);
			throw;
		}
	}
}