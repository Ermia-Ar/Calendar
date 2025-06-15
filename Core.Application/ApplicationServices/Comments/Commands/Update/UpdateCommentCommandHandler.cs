using Core.Application.ApplicationServices.Comments.Exceptions;
using Core.Domain.Interfaces;
using MediatR;

namespace Core.Application.ApplicationServices.Comments.Commands.Update;

public class UpdateCommentCommandHandler(IUnitOfWork unitOfWork, ICurrentUserServices currentUserServices)
            : IRequestHandler<UpdateCommentCommandRequest>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICurrentUserServices _currentUserServices = currentUserServices;

    public async Task Handle(UpdateCommentCommandRequest request, CancellationToken cancellationToken)
    {
        var userId = _currentUserServices.GetUserId();
        var comment = await _unitOfWork.Comments
            .FindById(request.Id, cancellationToken);

        if (comment.UserId != userId)
        {
            throw new OnlyCommentCreatorAllowedException();
        }

        comment.UpdateContent(request.Content);

        _unitOfWork.Comments.Update(comment);
        await _unitOfWork.SaveChangeAsync(cancellationToken);
    }
}
