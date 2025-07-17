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

namespace Core.Application.ApplicationServices.Projects.Commands.AddRecurringActivity;

public class AddRecurringActivityForProjectCommandHandler(
    ICurrentUserServices currentUserServices,
    IUnitOfWork unitOfWork)
    : IRequestHandler<AddRecurringActivityForProjectCommandRequest>
{
    private readonly ICurrentUserServices _currentUserServices = currentUserServices;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(AddRecurringActivityForProjectCommandRequest request, CancellationToken cancellationToken)
    {
        await using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
	        var owner = (await _unitOfWork.Users
			        .GetById(_currentUserServices.GetUserId(), cancellationToken))
		        .Adapt<GetUserByIdDto>();
	        long projectId = request.ProjectId;

	        //Generate Recurring Activities
	        var activities = GenerateRecurringActivities(owner.Id, projectId, request);
	        //add to activity table
	        activities = _unitOfWork.Activities.AddRange(activities);
	        await _unitOfWork.SaveChangeAsync(cancellationToken);

	        // for all membersIds
	        var activityMembers = new List<ActivityMember>();
	        var activityRequests = new List<ActivityRequest>();
	        foreach (var receiverId in request.MemberIds)
	        {
		        //check
		        var receiver = (await _unitOfWork
				        .Users.GetById(receiverId, cancellationToken))
			        .Adapt<GetUserByIdDto>();

		        if (receiver == null)
			        throw new NotFoundUserIdException(receiverId);

		        foreach (var activity in activities)
		        {
			        //create request for memberId
			        var activityRequest = ActivityRequestFactory.Create(receiverId,
				        activity.Id, "please join !");
			        activityRequests.Add(activityRequest);

			        var activityMember = ActivityMember
				        .Create(receiverId, activity.Id,
					        false, ParticipationStatus.Pending);
			        activityMembers.Add(activityMember);
		        }
	        }
	        //add To activityRequests table
	        _unitOfWork.ActivityRequests.AddRange(activityRequests);

	        foreach (var activity in activities)
	        {
		        //add owner to activity members
		        var activityOwner = ActivityMember
			        .Create(owner.Id, activity.Id,
				        false, ParticipationStatus.Participating);

		        // add to activityMembers 
		        activityMembers.Add(activityOwner);

		        // set notification for owner
		        // if NotificationBefore is null set default notification
		        var notificationBefore = request.NotificationBefore ??
		                                 owner.DefaultNotificationBefore;

		        var ownerNotification = NotificationFactory
			        .Create(owner.Id, activity.Id
				        , activity.StartDate - notificationBefore);

		        //Add notifications table
		        _unitOfWork.Notifications.Add(ownerNotification);


	        }
	        
	        //add To ActivityMembers Table
	        _unitOfWork.ActivityMembers.AddRange(activityMembers);

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
        AddRecurringActivityForProjectCommandRequest request)
    {
        var activities = new List<Activity>();
        var currentStart = request.StartDate;

        while (currentStart <= request.EndDate)
        {
            // create new activity
            var activity = ActivityFactory.Create(ownerId, projectId
                , request.Title, request.Description
                , currentStart, request.Type
                , request.Duration);

            activities.Add(activity);

            currentStart = currentStart.AddDays(request.Interval);
        }

        return activities;
    }
}