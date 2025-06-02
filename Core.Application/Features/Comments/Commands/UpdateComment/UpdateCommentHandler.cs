using Core.Application.Exceptions.Comment;
using Core.Domain;
using MediatR;

namespace Core.Application.Features.Comments.Commands.UpdateComment
{
    public class UpdateCommentHandler 
        : IRequestHandler<UpdateCommentCommand, string>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserServices _currentUserServices;

        public UpdateCommentHandler(IUnitOfWork unitOfWork, ICurrentUserServices currentUserServices)
        {
            _unitOfWork = unitOfWork;
            _currentUserServices = currentUserServices;
        }
        public async Task<string> Handle(UpdateCommentCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserServices.GetUserId();
            var comment = await _unitOfWork.Comments.GetCommentById(request.Id, cancellationToken);
            if (comment.UserId != userId)
            {
                throw new OnlyCommentCreatorAllowedException();
            }
            //update
            comment.Content = request.Content;
            comment.UpdatedDate = DateTime.Now;
            comment.IsEdited = true;
            _unitOfWork.Comments.UpdateComment(comment);
            await _unitOfWork.SaveChangeAsync(cancellationToken);
            return "Updated";
        }
    }
}
