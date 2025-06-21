using Core.Application.ApplicationServices.Activities.Exceptions;
using Core.Domain.Entity.Comments;
using Core.Domain.Interfaces;
using MediatR;

namespace Core.Application.ApplicationServices.Comments.Commands.Add;

public sealed class AddCommentCommandHandler(IUnitOfWork unitOfWork, ICurrentUserServices currentUserServices)
        : IRequestHandler<AddCommentCommandRequest>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICurrentUserServices _currentUserServices = currentUserServices;

    public async Task Handle(AddCommentCommandRequest request, CancellationToken cancellationToken)
    {
        string userId = _currentUserServices.GetUserId();

        var isMember = (await _unitOfWork.Requests.FindMemberIdsOfActivity
            (request.ActivityId, cancellationToken))
            .Any(Id => Id == userId);

        if (!isMember)
        {
            throw new OnlyActivityMembersAllowedException();
        }
        var activity = await _unitOfWork.Activities
            .FindById(request.ActivityId, cancellationToken);

        var comment = CommentFactory.Create(userId, request.ActivityId
            , activity.ProjectId, request.Content);

        _unitOfWork.Comments.Add(comment);
        await _unitOfWork.SaveChangeAsync(cancellationToken);
    }
}
