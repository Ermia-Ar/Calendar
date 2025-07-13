using Amazon.Runtime.SharedInterfaces;
using Core.Application.ApplicationServices.Auth.Exceptions;
using Core.Application.ApplicationServices.Projects.Exceptions;
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

namespace Core.Application.ApplicationServices.Projects.Commands.AddRecurringActivity;

public class AddRecurringActivityForProjectCommandHandler(
    ICurrentUserServices currentUserServices,
    IUnitOfWork unitOfWork)
    : IRequestHandler<AddRecurringActivityForProjectCommnadRequest>
{
    private readonly ICurrentUserServices _currentUserServices = currentUserServices;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(AddRecurringActivityForProjectCommnadRequest request, CancellationToken cancellationToken)
    {
        await using var transaction = await _unitOfWork.BeginTransaction(cancellationToken);
        try
        {
            var ownerId = _currentUserServices.GetUserId();
            var projectId = request.ProjectId;

            //for check if user is the member of project or not 
            var memberIds = await _unitOfWork.ProjectMembers
                .FindMemberIdsOfProject(projectId, cancellationToken);

            if (!memberIds.Any(x => x == ownerId))
                throw new OnlyProjectMembersAllowedException();

            //Generate Recurring Activities
            var activities = GenerateRecurringActivities(ownerId, projectId, request);

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

                foreach (var activity in activities)
                {
                    //check if receiver is member of base project
                    var isGuest = memberIds
                        .Any(x => x != receiverId);

                    //create request for memberId
                    var sendRequest = RequestFactory.Create(activity.Id
                        , ownerId, receiverId, request.Message, false);

                    userRequests.Add(sendRequest);
                }
            }

            //add requests table
            _unitOfWork.Requests.AddRange(userRequests);

            //
            //
            foreach (var activity in activities)
            {
                //add owner to activity members
                var activityOwner = ActivityMember
                    .CreateOwner(ownerId, activity.Id);

                // add to activityMembers table
                activityOwner = _unitOfWork.ActivityMembers.Add(activityOwner);

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
            await _unitOfWork.Commit(cancellationToken);
        }
        catch
        {
            await _unitOfWork.Rollback(cancellationToken);
            throw;
        }
    }

    private List<Activity> GenerateRecurringActivities(Guid ownerId, long projectId,
        AddRecurringActivityForProjectCommnadRequest request)
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