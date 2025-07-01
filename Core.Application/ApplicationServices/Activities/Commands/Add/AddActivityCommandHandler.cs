using Amazon.Runtime.Internal.Util;
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

namespace Core.Application.ApplicationServices.Activities.Commands.Add;

public sealed class AddActivityCommandHandler(
	IUnitOfWork unitOfWork,
	ICurrentUserServices currentUser,
	IConfiguration configuration
) : IRequestHandler<AddActivityCommandRequest, Dictionary<string, GetAllRequestQueryResponse>>
{
	private readonly IUnitOfWork _unitOfWork = unitOfWork;
	private readonly ICurrentUserServices _currentUser = currentUser;
	private readonly IConfiguration _configuration = configuration;

	public async Task<Dictionary<string, GetAllRequestQueryResponse>> Handle(AddActivityCommandRequest request, CancellationToken cancellationToken)
	{ 
		var ownerId = _currentUser.GetUserId();

		// create new activity
		var activity = ActivityFactory.Create(ownerId
			, request.Title, request.Description
			, request.StartDate, request.Category
			, request.Duration);

		// for signalr
		var response = new Dictionary<string, GetAllRequestQueryResponse>();

		//create request for all membersIds
		var userRequests = new List<Request>();

		foreach (var memberId in request.MemberIds)
		{
			//check
			var member = await _unitOfWork.Users.FindById(memberId);
			if (member == null)
			{
				throw new NotFoundUserIdException(memberId);
			}

			//create request for memberId
			var sendRequest = RequestFactory.CreateActivityRequest(_configuration["Public:ProjectId"], activity.Id
				, ownerId, member.Id, request.Message, false);

			userRequests.Add(sendRequest);

			//for signalR
			response[memberId] = sendRequest.Adapt<GetAllRequestQueryResponse>();
		}

		//add owner to activity members
		var ownerRequest = RequestFactory.CreateActivityRequest(
			_configuration["Public:ProjectId"]
			, activity.Id, ownerId, ownerId
			, request.Message, false);

		ownerRequest.Accept();
		ownerRequest.MakeArchived();

		// set notification for owner
		// if NotificationBefore is null set default notification
		var notificationBefore = request.NotificationBefore ??
			TimeSpan.FromHours(double.Parse(_configuration["Public:DefaultNotification"]));
		
		var notification = NotificationFactory
			.Create(ownerRequest.Id
			, activity.StartDate - notificationBefore);

		ownerRequest.SetNotification(notification.Id);

		//add to requests list
		userRequests.Add(ownerRequest);
	
		//add to activity table
		_unitOfWork.Activities.Add(activity);
		//add  requests
		_unitOfWork.Requests.AddRange(userRequests);
		//Add notification
		_unitOfWork.Notifications.Add(notification);

		await _unitOfWork.SaveChangeAsync(cancellationToken);

		return response;
	}
}