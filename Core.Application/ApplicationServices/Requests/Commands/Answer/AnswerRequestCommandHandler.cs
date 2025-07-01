using Core.Application.ApplicationServices.Requests.Exceptions;
using Core.Application.ApplicationServices.Requests.Queries.GetAll;
using Core.Application.Common;
using Core.Domain.Entities.Notifications;
using Core.Domain.Enum;
using Core.Domain.UnitOfWork;
using Mapster;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Core.Application.ApplicationServices.Requests.Commands.Answer;

public sealed class AnswerRequestCommandHandler(
	IUnitOfWork unitOfWork,
	ICurrentUserServices currentUserServices,
	IConfiguration configuration)
		: IRequestHandler<AnswerRequestCommandRequest, GetAllRequestQueryResponse>
{
    private readonly ICurrentUserServices _currentUserServices = currentUserServices;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IConfiguration _configuration = configuration;

	public async Task<GetAllRequestQueryResponse> Handle(AnswerRequestCommandRequest request, CancellationToken cancellationToken)
    {
        var userId = _currentUserServices.GetUserId();
        //get request
        var userRequest = await _unitOfWork.Requests
            .FindById(request.RequestId, cancellationToken);

        if (userRequest.ReceiverId != userId)
        {
            throw new NotFoundRequestException();
        }
        if (userRequest.IsExpire == true)
        {
            throw new ExpireRequestException();
        }

        //check if the request for a Project 
        if (userRequest.RequestFor == RequestFor.Project && request.IsAccepted == true)
        {
            userRequest.Accept();
        }
        else if (userRequest.RequestFor == RequestFor.Activity && request.IsAccepted == true)
        {
            var activity = await _unitOfWork.Activities
                .FindById(userRequest.ActivityId, cancellationToken);

            //set default notification 
            var notificationBefore 
                = TimeSpan.FromHours(double.Parse(_configuration["Public:DefaultNotification"]));

            var notification = NotificationFactory
                .Create(userRequest.Id, activity.StartDate - notificationBefore);

            userRequest.SetNotification(notification.Id);
            userRequest.Accept();
        }
        else
        {
            userRequest.Reject();
        }

        await _unitOfWork.SaveChangeAsync(cancellationToken);

        return userRequest.Adapt<GetAllRequestQueryResponse>();
    }
}
