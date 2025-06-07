using Core.Application.ApplicationServices.Activities.Exceptions;
using Core.Domain.Entity;
using Core.Domain.Interfaces;
using Mapster;
using MediatR;

namespace Core.Application.ApplicationServices.Comments.Commands.CreateComment;

public sealed class AddCommentCommandHandler
    : IRequestHandler<AddCommentCommandRequest>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserServices _currentUserServices;

    public AddCommentCommandHandler(IUnitOfWork unitOfWork, ICurrentUserServices currentUserServices)
    {
        _unitOfWork = unitOfWork;
        _currentUserServices = currentUserServices;
    }

    public async Task Handle(AddCommentCommandRequest request, CancellationToken cancellationToken)
    {
        string userId = _currentUserServices.GetUserId();

        var isMember = (await _unitOfWork.Requests.GetMemberOfActivity
            (request.ActivityId, cancellationToken))
            .Adapt<List<User>>()
            .Any(member => member.Id == userId);

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
        await _unitOfWork.Comments.AddComment(comment, cancellationToken);
        await _unitOfWork.SaveChangeAsync(cancellationToken);
    }
}
