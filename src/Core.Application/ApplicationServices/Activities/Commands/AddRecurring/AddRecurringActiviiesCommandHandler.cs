using Core.Application.ApplicationServices.Auth.Exceptions;
using Core.Application.Common;
using Core.Application.InternalServices.Auth.Dto;
using Core.Domain.Entities.Activities;
using Core.Domain.Entities.ActivityMembers;
using Core.Domain.Entities.Notifications;
using Core.Domain.Entities.Requests;
using Core.Domain.Enum;
using Core.Domain.Helper;
using Core.Domain.UnitOfWork;
using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Core.Application.ApplicationServices.Activities.Commands.AddRecurring;

public class AddRecurringActivityCommandHandler(
	ICurrentUserServices currentUserServices,
	IUnitOfWork unitOfWork,
	IConfiguration configuration)
	: IRequestHandler<AddRecurringActivityCommandRequest>
{
	private readonly ICurrentUserServices _currentUserServices = currentUserServices;
	private readonly IUnitOfWork _unitOfWork = unitOfWork;
	private readonly IConfiguration _configuration = configuration;
	public async Task Handle(AddRecurringActivityCommandRequest request, CancellationToken cancellationToken)
	{
		await using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

		try
		{
			var owner = (await _unitOfWork.Users
					.GetById(_currentUserServices.GetUserId(), cancellationToken))
				.Adapt<GetUserByIdDto>();
			long defaultProjectId = long.Parse(_configuration["PUBLIC:PROJECTID"]);

			//Generate Recurring Activities
			var activities = GenerateRecurringActivities(owner.Id, defaultProjectId, request);
			//add to activity table
			activities = _unitOfWork.Activities.AddRange(activities);
			await _unitOfWork.SaveChangeAsync(cancellationToken);

			// for all membersIds
			var activityMembers = new List<ActivityMember>(); 
			var activityRequests = new List<ActivityRequest>();
			foreach (var receiverId in request.MemberIds)
			{
				////check
				var receiver = (await _unitOfWork
					.Users.GetById(receiverId, cancellationToken))
					.Adapt<GetUserByIdDto>();

				if (receiver == null)
					throw new NotFoundUserIdException(receiverId);

				foreach (var activity in activities)
				{
					//create request for memberId
					var activityRequest = ActivityRequestFactory.Create( receiverId,
						activity.Id, "please join !");
					activityRequests.Add(activityRequest);

					var activityMember = ActivityMember
						.Create(receiverId, activity.Id,
							false, ParticipationStatus.Pending);
					activityMembers.Add(activityMember);
				}
			}
			//add To ActivityMembers Table
			_unitOfWork.ActivityMembers.AddRange(activityMembers);
			//add To activityRequests table
			_unitOfWork.ActivityRequests.AddRange(activityRequests);

			foreach (var activity in activities)
			{
				//add owner to activity members
				var activityOwner = ActivityMember
						.Create(owner.Id, activity.Id, 
							false, ParticipationStatus.Participating);

				// add to activityMembers table
				activityOwner = _unitOfWork.
					ActivityMembers.Add(activityOwner);

				await _unitOfWork.SaveChangeAsync(cancellationToken);

				// set notification for owner
				// if NotificationBefore is null set default notification
				var notificationBefore = request.NotificationBefore ??
					owner.DefaultNotificationBefore;

				var notification = NotificationFactory
					.Create(owner.Id, activity.Id
					, activity.StartDate - notificationBefore);

				//Add notifications table
				notification = _unitOfWork.Notifications.Add(notification);

				
			}

			await _unitOfWork.SaveChangeAsync(cancellationToken);

			await _unitOfWork.CommitTransactionAsync(cancellationToken);
		}
		catch
		{
			await _unitOfWork.RoleBackTransactionAsync(cancellationToken);
			throw;
		}
	}

	private List<Activity> GenerateRecurringActivities(Guid ownerId, long projectId,
		AddRecurringActivityCommandRequest request)
	{
		

		var activities = new List<Activity>();
		var currentStart = request.StartDate;

		while (currentStart <= request.ToDate)
		{
			// create new activity
			var activity = ActivityFactory.Create(ownerId, projectId
				, request.Title, request.Description
				, currentStart, request.Type
				, request.Duration);

			activities.Add(activity);
			//add interval
			currentStart = currentStart.AddDays(request.Interval);
		}

		return activities;
	}


}


