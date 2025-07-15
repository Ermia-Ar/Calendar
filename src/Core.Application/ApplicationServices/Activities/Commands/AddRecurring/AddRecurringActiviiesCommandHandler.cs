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
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Core.Application.ApplicationServices.Activities.Commands.AddRecurring;

public class AddRecurringActivityCommandHandler(
	ICurrentUserServices currentUserServices,
	IUnitOfWork unitOfWork,
	IConfiguration configuration)
	: IRequestHandler<AddRecurringActivityCommnadRequest>
{
	public async Task Handle(AddRecurringActivityCommnadRequest request, CancellationToken cancellationToken)
	{
		await using var transaction = await unitOfWork.BeginTransaction(cancellationToken);

		try
		{
			var ownerId = currentUserServices.GetUserId();
			var defaultProjectId = long.Parse(configuration["PUBLIC:PROJECTID"]);

			//Generate Recurring Activities
			var activities = GenerateRecurringActivities(ownerId, defaultProjectId, request);
			//add to activity table
			activities = unitOfWork.Activities.AddRange(activities);
			await unitOfWork.SaveChangeAsync(cancellationToken);

			//create request for all membersIds
			var userRequests = new List<Request>();
			foreach (var receiverId in request.MemberIds)
			{
				////check
				var receiver = (await unitOfWork
					.Users.GetById(receiverId, cancellationToken))
					.Adapt<GetUserByIdResponse>();

				if (receiver == null)
					throw new NotFoundUserIdException(receiverId);

				foreach (var activity in activities)
				{
					//create request for memberId
					var sendRequest = RequestFactory.Create(activity.Id
						, ownerId, /*receiverId*/ Guid.NewGuid(), request.Message, false);

					userRequests.Add(sendRequest);
				}
			}

			//add requests table
			unitOfWork.Requests.AddRange(userRequests);

			foreach (var activity in activities)
			{
				//add owner to activity members
				var activityOwner = ActivityMember
					.CreateOwner(ownerId, activity.Id);

				// add to activityMembers table
				activityOwner = unitOfWork.
					ActivityMembers.Add(activityOwner);

				await unitOfWork.SaveChangeAsync(cancellationToken);

				// set notification for owner
				// if NotificationBefore is null set default notification
				var notificationBefore = request.NotificationBefore ??
					TimeSpan.FromHours(NotificationSetting.DefaultNotificaiton);

				var notification = NotificationFactory
					.Create(activityOwner.Id
					, activity.StartDate - notificationBefore);

				//Add notifications table
				notification = unitOfWork.Notifications.Add(notification);

				//set notification
				activityOwner.SetNotification(notification.Id);
			}

			await unitOfWork.SaveChangeAsync(cancellationToken);

			await unitOfWork.Commit(cancellationToken);
		}
		catch
		{
			await unitOfWork.Rollback(cancellationToken);
			throw;
		}
	}

	private List<Activity> GenerateRecurringActivities(Guid ownerId, long projectId,
		AddRecurringActivityCommnadRequest request)
	{
		

		var activities = new List<Activity>();
		var currentStart = request.StartDate;

		while (currentStart <= request.EndDate)
		{
			// create new activity
			var activity = ActivityFactory.Create(ownerId, projectId
				, request.Title, request.Description
				, currentStart, request.Category
				, request.Duration);

			activities.Add(activity);

			// قدم بعدی بر اساس نوع recurrence
			currentStart = request.Recurrence switch
			{
				RecurrenceType.Daily => currentStart.AddDays(request.Interval),
				RecurrenceType.Weekly => currentStart.AddDays(7 * request.Interval),
				RecurrenceType.Monthly => currentStart.AddMonths(request.Interval),
				RecurrenceType.Yearly => currentStart.AddYears(request.Interval),
				_ => throw new NotSupportedException($"Recurrence type {request.Recurrence} is not supported.")
			};
		}

		return activities;
	}


}


