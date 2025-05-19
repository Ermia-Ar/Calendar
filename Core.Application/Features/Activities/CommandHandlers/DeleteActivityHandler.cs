using AutoMapper;
using Core.Application.Features.Activities.Commands;
using Core.Application.Features.Exceptions;
using Core.Domain;
using Core.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.Application.Features.Activities.CommandHandlers
{
    public class DeleteActivityHandler : ResponseHandler
        , IRequestHandler<DeleteActivityCommand, Response<string>>
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

        public async Task<Response<string>> Handle(DeleteActivityCommand request, CancellationToken cancellationToken)
        {
            var activity = await _unitOfWork.Activities.GetByIdAsync(request.Id, cancellationToken);
            if (activity.UserId != _currentUser.GetUserId())
            {
                throw new BadRequestException("Only the owner of this activity has access to this section.");
            }
            await using var transaction = await _unitOfWork.Activities.BeginTransactionAsync();
            try
            {
                //remove from comments table 
                await DeleteRangeCommentByActivityId(request.Id, cancellationToken);
                //remove from UserRequests table
                await DeleteRangeRequestByActivityId(request.Id, cancellationToken);
                //remove from activities table
                await _unitOfWork.Activities.DeleteAsyncById(request.Id, cancellationToken);
                //
                await _unitOfWork.SaveChangeAsync(cancellationToken);
                await transaction.CommitAsync();
                return Deleted("");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new BadRequestException("something wrong!");
            }
        }

        private async Task DeleteRangeCommentByActivityId(string activityId, CancellationToken token)
        {
            var comments = await _unitOfWork.Comments.GetTableNoTracking()
                .Where(x => x.ActivityId == activityId)
                .ToListAsync(token);

            _unitOfWork.Comments.DeleteRange(comments);
        }

        private async Task DeleteRangeRequestByActivityId(string activityId , CancellationToken token)
        {
            var request = await _unitOfWork.Requests.GetTableNoTracking()
                .Where(x => x.ActivityId == activityId)
                .ToListAsync(token);

           _unitOfWork.Requests.DeleteRange(request);
        }
    }
}