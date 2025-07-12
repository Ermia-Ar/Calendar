using Core.Application.ApplicationServices.Comments.Exceptions;
using Core.Application.ApplicationServices.Comments.Queries.GetAll;
using Core.Application.Common;
using Core.Domain.UnitOfWork;
using Mapster;
using MediatR;
using System.Runtime.InteropServices;

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
            throw new OnlyCommentCreatorAllowedException();

        comment.UpdateContent(request.Content);

        await _unitOfWork.SaveChangeAsync(cancellationToken);
    }
}
