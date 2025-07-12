using Core.Application.ApplicationServices.Requests.Exceptions;
using Core.Application.Common;
using Core.Domain.Entities.Notifications;
using Core.Domain.Enum;
using Core.Domain.UnitOfWork;
using MediatR;

namespace Core.Application.ApplicationServices.Projects.Commands.Exiting;

public sealed class ExitingProjectCommandHandler(IUnitOfWork unitOfWork, ICurrentUserServices currentUserServices)
        : IRequestHandler<ExitingProjectCommandRequest>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICurrentUserServices _currentUserServices = currentUserServices;

    public async Task Handle(ExitingProjectCommandRequest request, CancellationToken cancellationToken)
    {
        var userId = _currentUserServices.GetUserId();

        var projectMember = await _unitOfWork.ProjectMembers
             .GetByUserIdAndProjectId(userId, request.ProjectId, cancellationToken);

        if (projectMember == null)
            throw new NotFoundMemberException("You are not a member of this project.");

        var activityMembers = await _unitOfWork.ActivityMembers
                .FindActivityMemberOfActivitiesForProjectForUserId(
                 userId, request.ProjectId, cancellationToken);

        //////
        if (request.Activities)
        {
            //find all notifications for Project activities
            var notifications = new List<Notification>();
            foreach (var activityMember in activityMembers)
            {
                var notification = await _unitOfWork.Notifications
                       .Find(activityMember.Id, cancellationToken);

                if (notification != null)
                    notifications.Add(notification);
            }
            _unitOfWork.Notifications.RemoveRange(notifications);

            _unitOfWork.ActivityMembers.RemoveRange(activityMembers.ToList());
        }
        else
        {
            foreach (var activityMember in activityMembers)
            {
                activityMember.MakeGuest();
            }
        }

        await _unitOfWork.SaveChangeAsync(cancellationToken);
    }
}
