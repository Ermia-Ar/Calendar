using Core.Application.ApplicationServices.Activities.Exceptions;
using Core.Application.ApplicationServices.Auth.Exceptions;
using Core.Application.ApplicationServices.Requests.Queries.GetAll;
using Core.Application.Common;
using Core.Domain.Entities.Activities;
using Core.Domain.Entities.Notifications;
using Core.Domain.Entities.Requests;
using Core.Domain.UnitOfWork;
using Mapster;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Core.Application.ApplicationServices.Activities.Commands.AddSubActivity;

public sealed class AddSubActivityCommandHandler(
	IUnitOfWork unitOfWork, ICurrentUserServices currentUser, IConfiguration configuration)
			: IRequestHandler<AddSubActivityCommandRequest, Dictionary<string, GetAllRequestQueryResponse>>
{
	private readonly IUnitOfWork _unitOfWork = unitOfWork;
	private readonly ICurrentUserServices _currentUser = currentUser;
	private readonly IConfiguration _configuration = configuration;

	public async Task<Dictionary<string, GetAllRequestQueryResponse>> Handle(AddSubActivityCommandRequest request, CancellationToken cancellationToken)
	{
		var ownerId = _currentUser.GetUserId();

		//get base activity
		var baseActivity = await _unitOfWork.Activities
			.FindById(request.ActivityId, cancellationToken);

		if (baseActivity.UserId != ownerId)
		{
			throw new OnlyActivityCreatorAllowedException();
		}

		// create sub activity
		var subActivity = ActivityFactory.CreateSubActivity(baseActivity.Id
			, ownerId, baseActivity.ProjectId
			, baseActivity.Title, request.Description
			, request.StartDate, baseActivity.Category
			, request.Duration);

		//for signalR
		var response = new Dictionary<string, GetAllRequestQueryResponse>();
		//send request for all memberIds
		var userRequests = new List<Request>();
		foreach (var memberId in request.MemberIds)
		{
			//check
			var member = await _unitOfWork.Users.FindById(memberId);
			if (member == null)
			{
				throw new NotFoundUserIdException(memberId);
			}

			var sendRequest = RequestFactory.CreateActivityRequest
				(subActivity.ProjectId, subActivity.Id
				, ownerId, memberId, null, false);

			userRequests.Add(sendRequest);
			//for signalR 
			response[memberId] = sendRequest.Adapt<GetAllRequestQueryResponse>();
		}

		//add owner to activity members
		var onwerRequest = RequestFactory.CreateActivityRequest(subActivity.ProjectId
			, subActivity.Id, ownerId, ownerId
			, null, false);

		onwerRequest.Accept();
		onwerRequest.MakeArchived();

		// create notification for owner
		// if NotificationBefore is null set default notification
		var notificationBefore = request.NotificationBefore ??
			TimeSpan.FromHours(double.Parse(_configuration["Public:DefaultNotification"]));

		var notification = NotificationFactory
			.Create(onwerRequest.Id
			, subActivity.StartDate - notificationBefore);

		onwerRequest.SetNotification(notification.Id);

		userRequests.Add(onwerRequest);

		//add to table activity
		_unitOfWork.Activities.Add(subActivity);
		//send all requests
		_unitOfWork.Requests.AddRange(userRequests);
		//Add Notifications
		_unitOfWork.Notifications.Add(notification);


		await _unitOfWork.SaveChangeAsync(cancellationToken);

		return response;
	}
}
