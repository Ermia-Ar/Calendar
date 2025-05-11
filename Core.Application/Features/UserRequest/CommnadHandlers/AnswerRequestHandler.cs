using Core.Domain.Entity;
using Core.Domain.Shared;
using MediatR;
using Core.Domain;
using Core.Application.Features.UserRequests.Commnads;

namespace Core.Application.Features.UserRequests.CommandHandlers
{
    public class AnswerRequestHandler : ResponseHandler
        , IRequestHandler<AnswerRequestCommand, Response<string>>
    {
        private ICurrentUserServices _currentUserServices;
        private IUnitOfWork _unitOfWork;

        public AnswerRequestHandler(IUnitOfWork unitOfWork, ICurrentUserServices currentUserServices)
        {
            _unitOfWork = unitOfWork;
            _currentUserServices = currentUserServices;
        }

        public async Task<Response<string>> Handle(AnswerRequestCommand request, CancellationToken cancellationToken)
        {
            //check request validation
            var userName = _currentUserServices.GetUserName();
            var result = await _unitOfWork.Requests.GetByIdAsync(request.RequestId, cancellationToken);
            if (result.Receiver != userName)
            {
                return BadRequest<string>("not found request !");
            }
            if (result.IsExpire == true)
            {
                return BadRequest<string>("this request was Expire !");
            }
            //check if the request for a Project 
            if (result.RequestFor == Domain.Enum.RequestFor.Project && request.IsAccepted == true)
            {
                //sent request for each activity
                var userRequests = new List<UserRequest>();
                var activityIds = await _unitOfWork.Activities.GetProjectActivityIds(result.ProjectId, cancellationToken);
                foreach (var activityId in activityIds)
                {
                    var sendRequest = new UserRequest
                    {
                        Id = Guid.NewGuid().ToString(),
                        InvitedAt = DateTime.Now,
                        AnsweredAt = DateTime.Now,
                        Status = Domain.Enum.RequestStatus.Accepted,
                        Sender = _currentUserServices.GetUserName(),
                        IsGuest = false,
                        IsExpire = true,
                        IsActive = true,
                        Receiver = userName,
                        Message = null,
                        RequestFor = Domain.Enum.RequestFor.Activity,
                        ActivityId = activityId,
                        ProjectId = result.ProjectId,
                    };
                    userRequests.Add(sendRequest);
                }
                await _unitOfWork.Requests.AddRangeAsync(userRequests, cancellationToken);

            }
            //
            await _unitOfWork.Requests.AnswerRequest(request.RequestId, request.IsAccepted, cancellationToken);
            await _unitOfWork.SaveChangeAsync(cancellationToken);
            return NoContent<string>();

        }
    }
}
