using Core.Application.ApplicationServices.Comments.Exceptions;
using Core.Application.Common;
using Core.Domain.UnitOfWork;
using MediatR;

namespace Core.Application.ApplicationServices.Comments.Commands.Remove;

public class DeleteCommentCommandHandler(ICurrentUserServices currentUserServices, IUnitOfWork unitOfWork)
            : IRequestHandler<DeleteCommentCommandRequest>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICurrentUserServices _currentUserServices = currentUserServices;

    public async Task Handle(DeleteCommentCommandRequest request, CancellationToken cancellationToken)
    {
        var userId = _currentUserServices.GetUserId();

        var comment = await _unitOfWork.Comments
            .FindById(request.Id, cancellationToken);

        if (comment.UserId != userId)
            throw new OnlyCommentCreatorAllowedException();

        _unitOfWork.Comments.Remove(comment);
        await _unitOfWork.SaveChangeAsync(cancellationToken);

    }
}
