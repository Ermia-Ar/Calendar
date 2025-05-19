using AutoMapper;
using Core.Application.Features.Exceptions;
using Core.Domain;
using Core.Domain.Entity;
using Core.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Core.Application.Features.Comments.Commands.CreateComment
{
    public sealed class CreateCommentHandler : ResponseHandler
        , IRequestHandler<CreateCommentCommand, Response<string>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserServices _currentUserServices;

        public CreateCommentHandler(IUnitOfWork unitOfWork, ICurrentUserServices currentUserServices)
        {
            _unitOfWork = unitOfWork;
            _currentUserServices = currentUserServices;
        }

        public async Task<Response<string>> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
        {
            string userId = _currentUserServices.GetUserId();
            string userName = _currentUserServices.GetUserName();

            var activity = await _unitOfWork.Activities.GetByIdAsync(request.ActivityId, cancellationToken);
            var isMember = (await _unitOfWork.Requests.GetMemberOfActivity
                (request.ActivityId, cancellationToken)).Any(memberName => memberName == userName);

            if (!isMember && activity.UserId != userId)
            {
                throw new BadRequestException("Only members of this activity can comment on it.");
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
            await _unitOfWork.Comments.AddAsync(comment, cancellationToken);
            await _unitOfWork.SaveChangeAsync(cancellationToken);
            return Created("");
        }
    }
}
