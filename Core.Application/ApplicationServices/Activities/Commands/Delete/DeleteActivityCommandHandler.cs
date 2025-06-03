using AutoMapper;
using Core.Application.ApplicationServices.Activities.Exceptions;
using Core.Domain;
using MediatR;

namespace Core.Application.ApplicationServices.Activities.Commands.DeleteActivity
{
    public class DeleteActivityCommandHandler
        : IRequestHandler<DeleteActivityCommandRequest>
    {
        public readonly IUnitOfWork _unitOfWork;
        public readonly ICurrentUserServices _currentUser;
        public readonly IMapper _mapper;

        public DeleteActivityCommandHandler(IUnitOfWork unitOfWork, ICurrentUserServices currentUser, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
            _mapper = mapper;
        }

        public async Task Handle(DeleteActivityCommandRequest request, CancellationToken cancellationToken)
        {
            var activity = await _unitOfWork.Activities.GetActivityById(request.Id, cancellationToken);
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
            var comments = await _unitOfWork.Comments
                .GetComments(null, activityId, null, null, token);

            _unitOfWork.Comments.DeleteRangeComment(comments);
        }

        private async Task DeleteRangeRequestByActivityId(string activityId, CancellationToken token)
        {
            var request = await _unitOfWork.Requests
                .GetRequests(null, activityId, token);

            _unitOfWork.Requests.DeleteRangeRequests(request);
        }

        public async Task DeleteActivityById(string activityId, CancellationToken token)
        {
            var activity = await _unitOfWork.Activities
                .GetActivityById(activityId, token);

            _unitOfWork.Activities.DeleteActivity(activity);
        }
    }
}