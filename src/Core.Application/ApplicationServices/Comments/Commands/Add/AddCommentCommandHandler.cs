using Core.Application.ApplicationServices.Activities.Exceptions;
using Core.Application.ApplicationServices.Comments.Queries.GetAll;
using Core.Application.Common;
using Core.Domain.Entities.Comments;
using Core.Domain.UnitOfWork;
using Mapster;
using MediatR;

namespace Core.Application.ApplicationServices.Comments.Commands.Add;

public sealed class AddCommentCommandHandler(IUnitOfWork unitOfWork, ICurrentUserServices currentUserServices)
		: IRequestHandler<AddCommentCommandRequest>
{
	private readonly IUnitOfWork _unitOfWork = unitOfWork;
	private readonly ICurrentUserServices _currentUserServices = currentUserServices;

	public async Task Handle(AddCommentCommandRequest request, CancellationToken cancellationToken)
	{
		var userId = _currentUserServices.GetUserId();

		var members = await _unitOfWork.ActivityMembers
			.FindMemberIdsOfActivity(request.ActivityId, cancellationToken);

		if (!members.Any(x => x == userId))
			throw new OnlyActivityMembersAllowedException();

		var activity = await _unitOfWork.Activities
			.FindById(request.ActivityId, cancellationToken);

		var comment = CommentFactory.Create(userId, request.ActivityId
			, activity.ProjectId, request.Content);

		_unitOfWork.Comments.Add(comment);
		await _unitOfWork.SaveChangeAsync(cancellationToken);

	}
}
