using Core.Application.ApplicationServices.Activities.Exceptions;
using Core.Application.ApplicationServices.Auth.Exceptions;
using Core.Application.ApplicationServices.Requests.Exceptions;
using Core.Application.ApplicationServices.Requests.Queries.GetAll;
using Core.Application.Common;
using Core.Application.InternalServices.Auth.Dto;
using Core.Application.InternalServices.Auth.Services;
using Core.Domain.Entities.Requests;
using Core.Domain.Enum;
using Core.Domain.UnitOfWork;
using Mapster;
using MediatR;
using Microsoft.Extensions.Configuration;


namespace Core.Application.ApplicationServices.Activities.Commands.SubmitRequest;

public sealed class SubmitActivityRequestCommandHandler(
	IUnitOfWork unitOfWork,
	ICurrentUserServices currentUserServices,
	IConfiguration configuration)
		: IRequestHandler<SubmitActivityRequestCommandRequest>
{
	private readonly IConfiguration _configuration = configuration;
	private readonly ICurrentUserServices _currentUserServices = currentUserServices;
	private readonly IUnitOfWork _unitOfWork = unitOfWork;

	public async Task Handle(SubmitActivityRequestCommandRequest request, CancellationToken cancellationToken)
	{
		var senderId = _currentUserServices.GetUserId();

		var activity = await _unitOfWork.Activities
			.FindById(request.ActivityId, cancellationToken);

		if (activity == null)
			throw new InvalidActivityIdException();

		if (activity.UserId != senderId)
			throw new OnlyActivityCreatorAllowedException();

		//get memberIds of base project of activity
		var projectMemberIds = (await _unitOfWork.ProjectMembers
			.FindMemberIdsOfProject(activity.ProjectId, cancellationToken))
			.ToList();

		//send for each receivers
		var userRequests = new List<Request>();
		foreach (var receiverId in request.MemberIds)
		{
			////check
			var receiver = (await _unitOfWork
					.Users.GetById(receiverId, cancellationToken))
					.Adapt<GetUserByIdResponse>();

			if (receiver == null)
				throw new NotFoundUserIdException(receiverId);
			// receiverId
			var isMember = await _unitOfWork.ActivityMembers
				.IsMemberOfActivity(receiverId, activity.Id, cancellationToken);

			if (isMember)
				throw new TheUserAlreadyIsMemberActivity(receiverId);

			// check if the receiver is a member of base project
			var	isGuest = projectMemberIds.Any(x => x != receiverId);

			//create request
			var sendRequest = RequestFactory.Create(activity.Id,
				senderId, receiverId, request.Message,
				isGuest);

			userRequests.Add(sendRequest);
		}

		_unitOfWork.Requests.AddRange(userRequests);

		await _unitOfWork.SaveChangeAsync(cancellationToken);
	}
}
