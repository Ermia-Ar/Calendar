using Core.Domain.Entity;
using Core.Domain.Shared;
using MediatR;
using Core.Domain;
using Core.Application.Features.UserRequests.Commnads;
using Core.Application.Features.Exceptions;

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
            //check request validation
            var userName = _currentUserServices.GetUserName();
            var result = await _unitOfWork.Requests.GetByIdAsync(request.RequestId, cancellationToken);
            if (result.Receiver != userName)
            {
                throw new NotFoundException("not found request !");
            }
            if (result.IsExpire == true)
            {
                throw new BadRequestException("this request was Expire !");
            }
            //check if the request for a Project 
            if (result.RequestFor == Domain.Enum.RequestFor.Project && request.IsAccepted == true)
            {
                //check if the user is member of this project 
                var isMember = (await _unitOfWork.Requests
                    .GetMemberOfProject(result.ProjectId, cancellationToken))
                    .Any(memberName => memberName == userName);

                //sent request for each activity
                var userRequests = new List<UserRequest>();
                var activityIds = await _unitOfWork.Activities.GetProjectActiveActivityIds(result.ProjectId, cancellationToken);
                foreach (var activityId in activityIds)
                {
                    var sendRequest = UserRequest.CreateUserRequest(activityId, result.ProjectId
                        , userName, result.Sender, null, !isMember
                        , Domain.Enum.RequestStatus.Accepted);

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
