using Core.Application.Features.Exceptions;
using Core.Domain;
using Core.Domain.Shared;
using MediatR;

namespace Core.Application.Features.Comments.Commands.UpdateComment
{
    public class UpdateCommentHandler : ResponseHandler
        , IRequestHandler<UpdateCommentCommand, Response<string>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserServices _currentUserServices;

        public UpdateCommentHandler(IUnitOfWork unitOfWork, ICurrentUserServices currentUserServices)
        {
            _unitOfWork = unitOfWork;
            _currentUserServices = currentUserServices;
        }
        public async Task<Response<string>> Handle(UpdateCommentCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserServices.GetUserId();
            var comment = await _unitOfWork.Comments.GetByIdAsync(request.Id, cancellationToken);
            if (comment.UserId != userId)
            {
                throw new BadRequestException("Only its creator van update it");
            }
            //update
            comment.Content = request.Content;
            comment.UpdatedDate = DateTime.Now;
            comment.IsEdited = true;
            _unitOfWork.Comments.Update(comment);
            await _unitOfWork.SaveChangeAsync(cancellationToken);
            return Success("");
        }
    }
}
