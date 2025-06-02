using Core.Application.Exceptions.Activity;
using Core.Domain;
using Core.Domain.Entity;
using MediatR;

namespace Core.Application.Features.Comments.Commands.CreateComment;

public sealed class CreateCommentHandler 
    : IRequestHandler<CreateCommentCommand, string>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserServices _currentUserServices;

    public CreateCommentHandler(IUnitOfWork unitOfWork, ICurrentUserServices currentUserServices)
    {
        _unitOfWork = unitOfWork;
        _currentUserServices = currentUserServices;
    }

    public async Task<string> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
    {
        string userId = _currentUserServices.GetUserId();

        var isMember = (await _unitOfWork.Requests.GetMemberOfActivity
            (request.ActivityId, cancellationToken))
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
        return "Created";
    }
}
