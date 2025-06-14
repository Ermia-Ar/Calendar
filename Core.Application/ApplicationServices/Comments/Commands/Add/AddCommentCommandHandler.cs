using Core.Application.ApplicationServices.Activities.Exceptions;
using Core.Domain.Entity;
using Core.Domain.Interfaces;
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
        string userId = _currentUserServices.GetUserId();

        var isMember = (await _unitOfWork.Requests.GetMemberIdsOfActivity
            (request.ActivityId, cancellationToken))
            .Any(Id => Id == userId);

        if (!isMember)
        {
            throw new OnlyActivityMembersAllowedException();
        }

        var comment = new Comment
        {
            Id = Guid.NewGuid().ToString(),
            ProjectId = request.ProjectId,
            ActivityId = request.ActivityId,
            Content = request.Content,
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow,
            UserId = userId,
        };
        _unitOfWork.Comments.Add(comment);
        await _unitOfWork.SaveChangeAsync(cancellationToken);
    }
}
