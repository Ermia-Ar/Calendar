using Core.Application.Exceptions.Comment;
using Core.Domain;
using MediatR;

namespace Core.Application.Features.Comments.Commands.DeleteComment
{
    public class DeleteCommentHandler 
        : IRequestHandler<DeleteCommentCommand, string>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserServices _currentUserServices;

        public DeleteCommentHandler(ICurrentUserServices currentUserServices, IUnitOfWork unitOfWork)
        {
            _currentUserServices = currentUserServices;
            _unitOfWork = unitOfWork;
        }

        public async Task<string> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserServices.GetUserId();
            var comment = await _unitOfWork.Comments.GetCommentById(request.Id, cancellationToken);
            if (comment.UserId != userId)
            {
                throw new OnlyCommentCreatorAllowedException();
            }

            _unitOfWork.Comments.DeleteComment(comment);
            await _unitOfWork.SaveChangeAsync(cancellationToken);
            return "Deleted";
        }
    }
}
