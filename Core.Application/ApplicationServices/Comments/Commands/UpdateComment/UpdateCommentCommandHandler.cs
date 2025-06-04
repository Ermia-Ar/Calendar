using Core.Application.ApplicationServices.Comments.Exceptions;
using Core.Domain;
using Core.Domain.Entity;
using Mapster;
using MediatR;

namespace Core.Application.ApplicationServices.Comments.Commands.UpdateComment
{
    public class UpdateCommentCommandHandler
        : IRequestHandler<UpdateCommentCommandRequest>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserServices _currentUserServices;

        public UpdateCommentCommandHandler(IUnitOfWork unitOfWork, ICurrentUserServices currentUserServices)
        {
            _unitOfWork = unitOfWork;
            _currentUserServices = currentUserServices;
        }
        public async Task Handle(UpdateCommentCommandRequest request, CancellationToken cancellationToken)
        {
            var userId = _currentUserServices.GetUserId();
            var comment =( await _unitOfWork.Comments
                .GetCommentById(request.Id, cancellationToken))
                .Adapt<Comment>();

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
        }
    }
}
