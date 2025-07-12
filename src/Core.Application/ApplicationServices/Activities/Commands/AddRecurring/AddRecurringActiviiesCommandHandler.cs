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

public class AddRecurringActivityCommandHandler
	: IRequestHandler<AddRecurringActivityCommnadRequest>
{

	private readonly IUnitOfWork _unitOfWork;
	private readonly IConfiguration _configuration;
	private readonly ICurrentUserServices _currentUser;

	public AddRecurringActivityCommandHandler(ICurrentUserServices currentUserServices,
		IUnitOfWork unitOfWork, IConfiguration configuration)
	{
		_currentUser = currentUserServices;
		_unitOfWork = unitOfWork;
		_configuration = configuration;
	}

	public async Task Handle(AddRecurringActivityCommnadRequest request, CancellationToken cancellationToken)
	{
		var ownerId = _currentUser.GetUserId();
		var defaultProjectId = long.Parse(_configuration["PUBLIC:PROJECTID"]);

		//Generate Recurring Activities
		var activities = GenerateRecurringActivities(ownerId, defaultProjectId, request);
		//add to activity table
		activities = _unitOfWork.Activities.AddRange(activities);
		await _unitOfWork.SaveChangeAsync(cancellationToken);

		//create request for all membersIds
		var userRequests = new List<Request>();
		foreach (var receiverId in request.MemberIds)
		{
			//check
			var receiver = (await _unitOfWork
				.Users.GetById(receiverId, cancellationToken))
				.Adapt<GetUserByIdResponse>();

			if (receiver == null)
				throw new NotFoundUserIdException(receiverId);

			foreach(var activity in activities)
			{
				//create request for memberId
				var sendRequest = RequestFactory.Create(activity.Id
					, ownerId, receiverId, request.Message, false);

				userRequests.Add(sendRequest);
			}
		}

		//add requests table
		_unitOfWork.Requests.AddRange(userRequests);

		foreach (var activity in activities)
		{
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
		} 

		await _unitOfWork.SaveChangeAsync(cancellationToken);
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
				, request.StartDate, request.Category
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


