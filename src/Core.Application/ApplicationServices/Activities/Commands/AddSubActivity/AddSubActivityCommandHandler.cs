using Core.Application.ApplicationServices.Activities.Exceptions;
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

namespace Core.Application.ApplicationServices.Activities.Commands.AddSubActivity;

public sealed class AddSubActivityCommandHandler(
	IUnitOfWork unitOfWork,
	ICurrentUserServices currentUser)
	: IRequestHandler<AddSubActivityCommandRequest>
{
	private readonly IUnitOfWork _unitOfWork = unitOfWork;
	private readonly ICurrentUserServices _currentUser = currentUser;

	public async Task Handle(AddSubActivityCommandRequest request, CancellationToken cancellationToken)
	{
		await using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

		try
		{
			var owner = (await _unitOfWork.Users
					.GetById(_currentUser.GetUserId(), cancellationToken))
				.Adapt<GetUserByIdDto>();

			//get base activity
			var baseActivity = await _unitOfWork.Activities
				.FindById(request.ActivityId, cancellationToken);

			if (baseActivity == null)
				throw new InvalidActivityIdException();

			if (baseActivity.OwnerId != owner.Id)
				throw new OnlyActivityCreatorAllowedException();

			long parentId = baseActivity.ParentId?? baseActivity.Id;

			// create sub activity
			var subActivity = ActivityFactory.CreateSubActivity(parentId
				, owner.Id, baseActivity.ProjectId
				, request.Title, request.Description
				, request.StartDate, request.Type
				, request.Duration);

			//add to table activity
			subActivity = _unitOfWork.Activities.Add(subActivity);
			await _unitOfWork.SaveChangeAsync(cancellationToken);

			//get memberIds of base project of baseActivity
			var projectMemberIds = (await _unitOfWork.ProjectMembers
				.FindMemberIdsOfProject(baseActivity.ProjectId, cancellationToken))
				.ToList();

			// all memberIds
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

				
				// create activityRequest
				var activityRequest = ActivityRequestFactory.Create(receiver.Id, subActivity.Id,
						"Please Join !!");
				activityRequests.Add(activityRequest);
					
				// check if the receiver is a member of base project
				var isGuest = projectMemberIds.Any(x => x != receiver.Id);
				
				//create ActivityMember
				var activityMember = ActivityMember.Create(receiver.Id,
					subActivity.Id, isGuest, ParticipationStatus.Pending);
				activityMembers.Add(activityMember);
			}
			// add to activityMembers Table	
			_unitOfWork.ActivityMembers.AddRange(activityMembers);	
			//add ActivityRequests table
			_unitOfWork.ActivityRequests.AddRange(activityRequests);

			//add owner to activity members
			var activityOwner = ActivityMember
				.Create(owner.Id, subActivity.Id,
					false, ParticipationStatus.Participating);

			// add to activityMembers table
			 _unitOfWork.ActivityMembers.Add(activityOwner);

			//create notification for owner
			// if NotificationBefore is null set default notification
			var notificationBefore = request.NotificationBefore??
			                         owner.DefaultNotificationBefore;

			var notification = NotificationFactory
				.Create(owner.Id, subActivity.Id,
					 subActivity.StartDate - notificationBefore);

			//Add to Notifications table
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
