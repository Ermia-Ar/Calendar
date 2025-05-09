using AutoMapper;
using Core.Application.Features.Activities.Commands;
using Core.Domain;
using Core.Domain.Shared;
using MediatR;

namespace Core.Application.Features.Activities.CommandHandlers
{
    public class DeleteActivityHandler : ResponseHandler
        , IRequestHandler<DeleteActivityCommand, Response<string>>
    {
        public IUnitOfWork _unitOfWork;
        public ICurrentUserServices _currentUser;
        public IMapper _mapper;

        public DeleteActivityHandler(IUnitOfWork unitOfWork, ICurrentUserServices currentUser, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
            _mapper = mapper;
        }

        public async Task<Response<string>> Handle(DeleteActivityCommand request, CancellationToken cancellationToken)
        {
            var isFor = await _unitOfWork.Activities.IsActivityForUser(request.Id, _currentUser.GetUserId(), cancellationToken);
            if (!isFor)
            {
                return NotFound<string>("Activity not found !!");
            }
            await using var transaction = await _unitOfWork.Activities.BeginTransactionAsync();
            try
            {
                //remove from UserRequests
                await _unitOfWork.Requests.DeleteRangeByActivityId(request.Id, cancellationToken);
                //remove from activities table
                var userId = _currentUser.GetUserId();
                await _unitOfWork.Activities.DeleteAsyncById(request.Id, cancellationToken);
                await _unitOfWork.SaveChangeAsync(cancellationToken);
                await transaction.CommitAsync();
                return Deleted("");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return NotFound<string>(ex.Message);
            }
        }
    }
}