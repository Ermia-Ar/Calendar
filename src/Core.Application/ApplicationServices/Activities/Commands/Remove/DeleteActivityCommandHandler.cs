using Core.Application.ApplicationServices.Activities.Exceptions;
using Core.Application.Common;
using Core.Domain.Entities.Notifications;
using Core.Domain.UnitOfWork;
using MediatR;

namespace Core.Application.ApplicationServices.Activities.Commands.Remove;

public sealed class DeleteActivityCommandHandler(IUnitOfWork unitOfWork, ICurrentUserServices currentUser)
            : IRequestHandler<DeleteActivityCommandRequest>
{
    public readonly IUnitOfWork _unitOfWork = unitOfWork;
    public readonly ICurrentUserServices _currentUser = currentUser;

    public async Task Handle(DeleteActivityCommandRequest request, CancellationToken cancellationToken)
    {
        var activity = await _unitOfWork.Activities
                .FindById(request.Id, cancellationToken);

        //if (activity == null)
        //    throw new InvalidActivityIdException();
        
        //if (activity.UserId != _currentUser.GetUserId())
        //    throw new OnlyActivityCreatorAllowedException();
        
        //remove from comments table 
        await DeleteRangeCommentByActivityId(activity.Id, cancellationToken);
        //remove from UserRequests table
        await DeleteRangeActivityMembersNotificationByActivityId(activity.Id, cancellationToken);
        //remove from activities table
        _unitOfWork.Activities.Remove(activity);

        //
        await _unitOfWork.SaveChangeAsync(cancellationToken);

    }

    private async Task DeleteRangeCommentByActivityId(long activityId, CancellationToken token)
    {
        var comments = await _unitOfWork.Comments
            .Find(null, activityId, token);

        _unitOfWork.Comments.RemoveRange(comments);
    }

    private async Task DeleteRangeActivityMembersNotificationByActivityId(long activityId, CancellationToken token)
    {
        var activiyMembers = (await _unitOfWork.ActivityMembers
            .FindByActivityId(activityId, token))
            .ToList();

        var notifications = new List<Notification>();
        foreach (var activiyMember in activiyMembers)
        {
            var notification =
                await _unitOfWork.Notifications.Find(activiyMember.Id, token);
            if (notification != null)
                notifications.Add(notification);
        }
        _unitOfWork.Notifications.RemoveRange(notifications);
        _unitOfWork.ActivityMembers.RemoveRange(activiyMembers);
    }
}