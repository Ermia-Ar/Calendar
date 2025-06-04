using Core.Application.ApplicationServices.Comments.Exceptions;
using Core.Application.ApplicationServices.Comments.Queries.GetComments;
using Core.Domain;
using Core.Domain.Entity;
using Mapster;
using MediatR;

namespace Core.Application.ApplicationServices.Comments.Commands.DeleteComment
{
    public class DeleteCommentCommandHandler
        : IRequestHandler<DeleteCommentCommandRequest>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserServices _currentUserServices;

        public DeleteCommentCommandHandler(ICurrentUserServices currentUserServices, IUnitOfWork unitOfWork)
        {
            _currentUserServices = currentUserServices;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(DeleteCommentCommandRequest request, CancellationToken cancellationToken)
        {
            var userId = _currentUserServices.GetUserId();
            var comment = (await _unitOfWork.Comments
                .GetCommentById(request.Id, cancellationToken))
                .Adapt<Comment>();

            if (comment.UserId != userId)
            {
                throw new OnlyCommentCreatorAllowedException();
            }

            _unitOfWork.Comments.DeleteComment(comment);
            await _unitOfWork.SaveChangeAsync(cancellationToken);
        }
    }
}
