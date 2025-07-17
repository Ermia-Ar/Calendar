using Core.Application.ApplicationServices.Auth.Exceptions;
using Core.Application.ApplicationServices.Projects.Exceptions;
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
			var owner =(await _unitOfWork.Users
					.GetById(_currentUser.GetUserId(), cancellationToken))
				.Adapt<GetUserByIdDto>();

			//for check if user is the member of project or not 
			var memberIds = await _unitOfWork.ProjectMembers
				.FindMemberIdsOfProject(request.ProjectId, cancellationToken);

			if (!memberIds.Any(x => x == owner.Id))
				throw new OnlyProjectMembersAllowedException();

			// create activity for project
			var activity = ActivityFactory.Create(owner.Id, request.ProjectId
				, request.Title, request.Description
				, request.StartDate, request.Type
				, request.Duration);

			// add to activity table
			activity = _unitOfWork.Activities.Add(activity);
			await _unitOfWork.SaveChangeAsync(cancellationToken);
			
			// for activity members 
			var activityMembers = new List<ActivityMember>();
			//for activity requests
			var activityRequests = new List<ActivityRequest>();
			foreach (var receiverId in request.MemberIds)
			{
				//check
				var receiver = (await _unitOfWork
						.Users.GetById(receiverId, cancellationToken))
						.Adapt<GetUserByIdDto>();

				if (receiver == null)
					throw new NotFoundUserIdException(receiverId);

				//check if receiver is member of base project
				var isGuest = memberIds
					.Any(x => x != receiverId);

				// create request for memberId
				var activityRequest = ActivityRequestFactory.Create(receiverId, 
					activity.Id, "Please Join");
				activityRequests.Add(activityRequest);
				
				// create activityMembers For memberId
				var activityMember = ActivityMember.Create(receiverId,
					activity.Id, isGuest, ParticipationStatus.Pending);
				activityMembers.Add(activityMember);
			}

			//add activityRequests table
			_unitOfWork.ActivityRequests.AddRange(activityRequests);
			
			//add owner to activity members
			var activityOwner = ActivityMember
				.Create(owner.Id, activity.Id, 
					false, ParticipationStatus.Participating);
			
			// add to activityMembers
			 activityMembers.Add(activityOwner);

			// create notification for owner
			// if NotificationBefore is null set default notification
			var notificationBefore = request.NotificationBefore ?? 
			                         owner.DefaultNotificationBefore;

			var notification = NotificationFactory
				.Create(owner.Id, activity.Id,
					activity.StartDate - notificationBefore);

			//add To ActivityMembers Table
			_unitOfWork.ActivityMembers.AddRange(activityMembers);
			
			//add notifications table
			_unitOfWork.Notifications.Add(notification);

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