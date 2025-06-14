using AutoMapper;
using Core.Application.ApplicationServices.Activities.Exceptions;
using Core.Domain.Entity;
using Core.Domain.Interfaces;
using Mapster;
using MediatR;

namespace Core.Application.ApplicationServices.Activities.Commands.Remove
{
    public class DeleteActivityCommandHandler(IUnitOfWork unitOfWork, ICurrentUserServices currentUser, IMapper mapper)
                : IRequestHandler<DeleteActivityCommandRequest>
    {
        public readonly IUnitOfWork _unitOfWork = unitOfWork;
        public readonly ICurrentUserServices _currentUser = currentUser;
        public readonly IMapper _mapper = mapper;

        public async Task Handle(DeleteActivityCommandRequest request, CancellationToken cancellationToken)
        {
            var activity = await _unitOfWork.Activities
                .FindById(request.Id, cancellationToken);

            if (activity.UserId != _currentUser.GetUserId())
            {
                throw new OnlyActivityCreatorAllowedException();
            }
            //remove from comments table 
            await DeleteRangeCommentByActivityId(request.Id, cancellationToken);
            //remove from UserRequests table
            await DeleteRangeRequestByActivityId(request.Id, cancellationToken);
            //remove from activities table
            await DeleteActivityById(request.Id, cancellationToken);

            //
            await _unitOfWork.SaveChangeAsync(cancellationToken);
        }

        private async Task DeleteRangeCommentByActivityId(string activityId, CancellationToken token)
        {
            var comments = (await _unitOfWork.Comments
                .GetAll(null, activityId, null, null, token))
                .Adapt<List<Comment>>();

            _unitOfWork.Comments.RemoveRange(comments);
        }

        private async Task DeleteRangeRequestByActivityId(string activityId, CancellationToken token)
        {
            var request = (await _unitOfWork.Requests
                .GetAll(null, activityId,null, null,null, token))
                .Adapt<List<UserRequest>>();

            _unitOfWork.Requests.RemoveRange(request);
        }

        public async Task DeleteActivityById(string activityId, CancellationToken token)
        {
            var activity = (await _unitOfWork.Activities
                .GetById(activityId, token))
                .Adapt<Activity>();

            _unitOfWork.Activities.Delete(activity);
        }
    }
}