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

        if (activity.UserId != _currentUser.GetUserId())
        {
            throw new OnlyActivityCreatorAllowedException();
        }
        //remove from comments table 
        await DeleteRangeCommentByActivityId(request.Id, cancellationToken);
        //remove from UserRequests table
        await DeleteRangeRequestAndNotificationByActivityId(request.Id, cancellationToken);
        //remove from activities table
        await DeleteActivityById(request.Id, cancellationToken);

        //
        await _unitOfWork.SaveChangeAsync(cancellationToken);
    }

    private async Task DeleteRangeCommentByActivityId(string activityId, CancellationToken token)
    {
        var comments = await _unitOfWork.Comments
            .Find(null, activityId, token);

        _unitOfWork.Comments.RemoveRange(comments);
    }

    private async Task DeleteRangeRequestAndNotificationByActivityId(string activityId, CancellationToken token)
    {
        //
        var requests = (await _unitOfWork.Requests
            .Find(null, activityId, null, null, null, token))
            .ToList();

        var notifications = new List<Notification>();
        foreach (var request in requests)
        {
            var notification =
                await _unitOfWork.Notifications.Find(request.Id, token);
            if (notification != null)
                notifications.Add(notification);
        }
        _unitOfWork.Notifications.RemoveRange(notifications);
        _unitOfWork.Requests.RemoveRange(requests);
    }

    public async Task DeleteActivityById(string activityId, CancellationToken token)
    {
        var activity = await _unitOfWork.Activities
            .FindById(activityId, token);

        _unitOfWork.Activities.Delete(activity);
    }
}