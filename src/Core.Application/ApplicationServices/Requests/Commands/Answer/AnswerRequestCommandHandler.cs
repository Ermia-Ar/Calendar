using Core.Application.ApplicationServices.Activities.Exceptions;
using Core.Application.ApplicationServices.Requests.Exceptions;
using Core.Application.Common;
using Core.Domain.Entities.ActivityMembers;
using Core.Domain.Entities.Notifications;
using Core.Domain.Helper;
using Core.Domain.UnitOfWork;
using MediatR;

namespace Core.Application.ApplicationServices.Requests.Commands.Answer;

public sealed class AnswerRequestCommandHandler(
	IUnitOfWork unitOfWork,
	ICurrentUserServices currentUserServices)
		: IRequestHandler<AnswerRequestCommandRequest>
{
    private readonly ICurrentUserServices _currentUserServices = currentUserServices;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

	public async Task Handle(AnswerRequestCommandRequest request, CancellationToken cancellationToken)
    {
        var userId = _currentUserServices.GetUserId();
        //get request
        var userRequest = await _unitOfWork.Requests
            .FindById(request.RequestId, cancellationToken);

        if (userRequest is null)
            throw new InvalidRequestIdException();

        if (userRequest.ReceiverId != userId)
            throw new NotFoundRequestException();
        
        if (userRequest.IsExpire == true)
            throw new ExpireRequestException();

    
        if (request.IsAccepted == true)
        {
            var activity = await _unitOfWork.Activities
                .FindById(userRequest.ActivityId, cancellationToken);
            
            if (activity == null)
                throw new InvalidActivityIdException();

            //Accept request
			userRequest.Accept();

            //Check if the user is already is member of Activitiy or not
            var isAlreadyMember = await _unitOfWork.ActivityMembers
                .IsMemberOfActivity(userRequest.ReceiverId, userRequest.ActivityId,
                cancellationToken);

            if (!isAlreadyMember)
            {
                //create
				var activityMember = ActivityMember
					.Create(userRequest.ReceiverId, userRequest.ActivityId, userRequest.IsGuest);

				//add to ActivityMember Table
				activityMember = _unitOfWork.ActivityMembers.Add(activityMember);
				await _unitOfWork.SaveChangeAsync(cancellationToken);

				//set default notification 
				var notificationBefore
                    = TimeSpan.FromHours(NotificationSetting.DefaultNotificaiton);

                var defaultNotification = activity.StartDate - notificationBefore;

                var notification = NotificationFactory
                    .Create(activityMember.Id, defaultNotification);

				notification = _unitOfWork.Notifications.Add(notification);

				activityMember.SetNotification(notification.Id);
			}
        }
        else
        {
            userRequest.Reject();
        }
        
        await _unitOfWork.SaveChangeAsync(cancellationToken);

    }
}
