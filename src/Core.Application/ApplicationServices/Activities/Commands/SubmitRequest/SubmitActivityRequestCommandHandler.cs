using Core.Application.ApplicationServices.Activities.Exceptions;
using Core.Application.ApplicationServices.Auth.Exceptions;
using Core.Application.Common;
using Core.Application.InternalServices.Auth.Dto;
using Core.Domain.Entities.ActivityMembers;
using Core.Domain.Entities.Requests;
using Core.Domain.Enum;
using Core.Domain.UnitOfWork;
using Mapster;
using MediatR;


namespace Core.Application.ApplicationServices.Activities.Commands.SubmitRequest;

public sealed class SubmitActivityRequestCommandHandler(
	IUnitOfWork unitOfWork,
	ICurrentUserServices currentUserServices)
		: IRequestHandler<SubmitActivityRequestCommandRequest>
{
	private readonly ICurrentUserServices _currentUserServices = currentUserServices;
	private readonly IUnitOfWork _unitOfWork = unitOfWork;

	public async Task Handle(SubmitActivityRequestCommandRequest request, CancellationToken cancellationToken)
	{
		var senderId = _currentUserServices.GetUserId();

		var activity = await _unitOfWork.Activities
			.FindById(request.ActivityId, cancellationToken);

		if (activity == null)
			throw new InvalidActivityIdException();

		if (activity.OwnerId != senderId)
			throw new OnlyActivityCreatorAllowedException();

		//get memberIds of base project of activity
		var projectMemberIds = (await _unitOfWork.ProjectMembers
			.FindMemberIdsOfProject(activity.ProjectId, cancellationToken))
			.ToList();

		//send for each receiver
		var activityRequests = new List<ActivityRequest>();
		var activityMembers = new List<ActivityMember>();

		foreach (var receiverId in request.MemberIds)
		{
			//check
			var receiver = (await _unitOfWork
					.Users.GetById(receiverId, cancellationToken))
				.Adapt<GetUserByIdDto>();

			if (receiver == null)
				throw new NotFoundUserIdException(receiverId);

			// receiverId
			var isMember = await _unitOfWork.ActivityMembers
				.IsMemberOfActivity(receiverId, activity.Id, cancellationToken);

			if (isMember)
				throw new TheUserAlreadyIsMemberActivity(receiverId);

			// check if the receiver is a member of base project
			var isGuest = projectMemberIds.Any(x => x != receiverId);

			//create request
			var activityRequest = ActivityRequestFactory.Create(senderId,
				activity.Id, "Please Join !!");
			activityRequests.Add(activityRequest);

			var activityMember = ActivityMember
				.Create(receiverId, activity.Id, isGuest, ParticipationStatus.Pending);
			activityMembers.Add(activityMember);
		}

		// Add to ActivityRequest Table
		_unitOfWork.ActivityRequests.AddRange(activityRequests);
		// add To ActivityRequest Table
		_unitOfWork.ActivityMembers.AddRange(activityMembers);
		//save change
		await _unitOfWork.SaveChangeAsync(cancellationToken);
	}
}
