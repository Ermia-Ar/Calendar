using Core.Application.ApplicationServices.Activities.Exceptions;
using Core.Application.ApplicationServices.Auth.Exceptions;
using Core.Application.ApplicationServices.Requests.Exceptions;
using Core.Application.ApplicationServices.Requests.Queries.GetAll;
using Core.Application.Common;
using Core.Domain.Entities.Requests;
using Core.Domain.Enum;
using Core.Domain.UnitOfWork;
using Mapster;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Core.Application.ApplicationServices.Activities.Commands.SubmitRequest;

public sealed class SubmitActivityRequestCommandHandler(IUnitOfWork unitOfWork, ICurrentUserServices currentUserServices, IConfiguration configuration)
			: IRequestHandler<SubmitActivityRequestCommandRequest, Dictionary<string, GetAllRequestQueryResponse>>
{
	private readonly IConfiguration _configuration = configuration;
	private readonly ICurrentUserServices _currentUserServices = currentUserServices;
	private readonly IUnitOfWork _unitOfWork = unitOfWork;

	public async Task<Dictionary<string, GetAllRequestQueryResponse>> Handle(SubmitActivityRequestCommandRequest request, CancellationToken cancellationToken)
	{
		var defaultProjectId = _configuration["Public:ProjectId"].ToString();
		var senderId = _currentUserServices.GetUserId();

		var activity = await _unitOfWork.Activities
			.FindById(request.ActivityId, cancellationToken);

		if (activity.UserId != senderId)
			throw new OnlyActivityCreatorAllowedException();

		//get memnerIds of base project of activity
		var projectMemberIds = await _unitOfWork.Requests
			.FindMemberIdsOfProject(activity.ProjectId, cancellationToken);
		
		//for signalR
		var response = new Dictionary<string, GetAllRequestQueryResponse>();
		//send for each Receivers
		var userRequests = new List<Request>();
		foreach (var receiverId in request.MemberIds)
		{
			var receiver = await _unitOfWork.Users.FindById(receiverId);
			if (receiver == null)
				throw new NotFoundUserIdException(receiverId);

			// TODO : check validation better
			//Check if such a request has already been filed for this recipient.
			var item = await _unitOfWork.Requests.Find(null, activity.Id
				, receiver.Id, null, null, cancellationToken);

			if (item.Any(x => x.Status == RequestStatus.Accepted || x.Status == RequestStatus.Pending))
				throw new SuchRequestHasAlreadyBeenFiledException(receiver.UserName);

			// check if the receiver is a member of base project
			bool isGuest = false;
			if (activity.ProjectId != defaultProjectId)
				isGuest = projectMemberIds.Any(x => x != receiverId);

			//create request
			var sendRequest = RequestFactory.CreateActivityRequest(activity.ProjectId, activity.Id
				, senderId, receiverId, request.Message, isGuest);

			userRequests.Add(sendRequest);
			//for signalR
			response[receiverId] = sendRequest.Adapt<GetAllRequestQueryResponse>();
		}

		_unitOfWork.Requests.AddRange(userRequests);

		await _unitOfWork.SaveChangeAsync(cancellationToken);
		return response;
	}
}
