using AutoMapper;
using Core.Application.Features.Exceptions;
using Core.Domain;
using Core.Domain.Shared;
using MediatR;

namespace Core.Application.Features.Comments.Commands.DeleteComment
{
    public class DeleteCommentHandler : ResponseHandler
        , IRequestHandler<DeleteCommentCommand, Response<string>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserServices _currentUserServices;

        public DeleteCommentHandler(ICurrentUserServices currentUserServices, IUnitOfWork unitOfWork)
        {
            _currentUserServices = currentUserServices;
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<string>> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserServices.GetUserId();
            var comment = await _unitOfWork.Comments.GetByIdAsync(request.Id, cancellationToken);
            if (comment.UserId != userId)
            {
                throw new BadRequestException("Only its creator can delete it.");
            }
            _unitOfWork.Comments.Delete(comment);
            await _unitOfWork.SaveChangeAsync(cancellationToken);
            return Deleted("");
        }
    }
}
