using Core.Domain.Entity;
using Core.Domain.Shared;
using MediatR;
using Core.Domain;
using Core.Application.Features.UserRequests.Commnads;
using Core.Application.Features.Exceptions;
using Core.Application.Features.Auth.Handler;
using Microsoft.AspNetCore.Identity;
using Core.Domain.Enum;

namespace Core.Application.Features.UserRequests.CommandHandlers
{
    public class AnswerRequestHandler : ResponseHandler
        , IRequestHandler<AnswerRequestCommand, Response<string>>
    {
        private readonly ICurrentUserServices _currentUserServices;
        private readonly IUnitOfWork _unitOfWork;

        public AnswerRequestHandler(IUnitOfWork unitOfWork, ICurrentUserServices currentUserServices)
        {
            _unitOfWork = unitOfWork;
            _currentUserServices = currentUserServices;
        }

        public async Task<Response<string>> Handle(AnswerRequestCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserServices.GetUserId();
            //get request
            var userRequest = await _unitOfWork.Requests.GetByIdAsync(request.RequestId, cancellationToken);
            if (userRequest.ReceiverId != userId)
            {
                throw new NotFoundException("not found request !");
            }
            if (userRequest.IsExpire == true)
            {
                throw new BadRequestException("this request was Expire !");
            }

            //check if the request for a Project 
            if (userRequest.RequestFor == Domain.Enum.RequestFor.Project && request.IsAccepted == true)
            {
                //sent request for each activity in project
                var userRequests = new List<UserRequest>();
                var activityIds = await _unitOfWork.Activities
                    .GetProjectActiveActivityIds(userRequest.ProjectId, cancellationToken);

                foreach (var activityId in activityIds)
                {
                    var sendRequest = UserRequest.CreateUserRequest(activityId
                        , userRequest.ProjectId, userRequest.SenderId
                        , userRequest.ReceiverId, null
                        , false, RequestStatus.Accepted);

                    userRequests.Add(sendRequest);
                }
                await _unitOfWork.Requests.AddRangeAsync(userRequests, cancellationToken);

            }
            //
            await _unitOfWork.Requests.AnswerRequest(request.RequestId, request.IsAccepted, cancellationToken);
            await _unitOfWork.SaveChangeAsync(cancellationToken);
            return Success("");

        }
    }
}
