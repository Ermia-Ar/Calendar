using AutoMapper;
using Core.Application.Exceptions.Activity;
using Core.Application.Features.Activities.Commands;
using Core.Domain;
using MediatR;

namespace Core.Application.Features.Activities.CommandHandlers
{
    public class DeleteActivityHandler 
        : IRequestHandler<DeleteActivityCommand, string>
    {
        public readonly IUnitOfWork _unitOfWork;
        public readonly ICurrentUserServices _currentUser;
        public readonly IMapper _mapper;

        public DeleteActivityHandler(IUnitOfWork unitOfWork, ICurrentUserServices currentUser, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
            _mapper = mapper;
        }

        public async Task<string> Handle(DeleteActivityCommand request, CancellationToken cancellationToken)
        {
            var activity = await _unitOfWork.Activities.GetActivityById(request.Id, cancellationToken);
            if (activity.UserId != _currentUser.GetUserId())
            {
                throw new OnlyActivityCreatorAllowedException();
            }
            await using var transaction = await _unitOfWork.BeginTransactionAsync();
            try
            {
                //remove from comments table 
                await DeleteRangeCommentByActivityId(request.Id, cancellationToken);
                //remove from UserRequests table
                await DeleteRangeRequestByActivityId(request.Id, cancellationToken);
                //remove from activities table
                await DeleteActivityById(request.Id, cancellationToken);
                //
                await _unitOfWork.SaveChangeAsync(cancellationToken);
                await transaction.CommitAsync();
                return "Deleted";
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception("something wrong!");
            }
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