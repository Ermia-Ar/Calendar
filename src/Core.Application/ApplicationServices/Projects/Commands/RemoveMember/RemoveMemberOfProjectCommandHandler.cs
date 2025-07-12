using Core.Application.ApplicationServices.Projects.Exceptions;
using Core.Application.ApplicationServices.Requests.Exceptions;
using Core.Application.Common;
using Core.Domain.Entities.ActivityMembers;
using Core.Domain.Entities.Notifications;
using Core.Domain.Enum;
using Core.Domain.UnitOfWork;
using MediatR;

namespace Core.Application.ApplicationServices.Projects.Commands.RemoveMember;

public sealed class RemoveMemberOfProjectCommandHandler(IUnitOfWork unitOfWork, ICurrentUserServices currentUserServices)
        : IRequestHandler<RemoveMemberOfProjectCommandRequest>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICurrentUserServices _currentUserServices = currentUserServices;

    public async Task Handle(RemoveMemberOfProjectCommandRequest request, CancellationToken cancellationToken)
    {
        //get current user id
        var userId = _currentUserServices.GetUserId();

        //check if user is creator of project or not
        var project = await _unitOfWork.Projects
            .FindById(request.ProjectId, cancellationToken);

        if (project == null)
            throw new InvalidProjectIdException();

        if (project.OwnerId != userId)
            throw new OnlyProjectCreatorAllowedException();

        var projectMember = await _unitOfWork.ProjectMembers
             .GetByUserIdAndProjectId(request.UserId, request.ProjectId,
             cancellationToken);

        if (projectMember == null)
            throw new NotFoundMemberException("No such member was found.");

        var activityMembers = await _unitOfWork.ActivityMembers
            .FindActivityMemberOfActivitiesForProjectForUserId(
            request.UserId, request.ProjectId, cancellationToken);

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
