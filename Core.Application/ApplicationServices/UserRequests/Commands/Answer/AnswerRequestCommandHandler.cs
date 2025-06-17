using Core.Application.ApplicationServices.UserRequests.Exceptions;
using Core.Domain.Entity;
using Core.Domain.Entity.UserRequests;
using Core.Domain.Enum;
using Core.Domain.Interfaces;
using Mapster;
using MediatR;

namespace Core.Application.ApplicationServices.UserRequests.Commands.Answer;

public class AnswerRequestCommandHandler
    : IRequestHandler<AnswerRequestCommandRequest>
{
    private readonly ICurrentUserServices _currentUserServices;
    private readonly IUnitOfWork _unitOfWork;

    public AnswerRequestCommandHandler(IUnitOfWork unitOfWork, ICurrentUserServices currentUserServices)
    {
        _unitOfWork = unitOfWork;
        _currentUserServices = currentUserServices;
    }

    public async Task Handle(AnswerRequestCommandRequest request, CancellationToken cancellationToken)
    {
        var userId = _currentUserServices.GetUserId();
        //get request
        var userRequest = await _unitOfWork.Requests
            .FindById(request.RequestId, cancellationToken);

        if (userRequest.ReceiverId != userId)
        {
            throw new NotFoundRequestException();
        }
        if (userRequest.IsExpire == true)
        {
            throw new ExpireRequestException();
        }

        //check if the request for a Project 
        if (userRequest.RequestFor == RequestFor.Project && request.IsAccepted == true)
        {

            // if the user is already member of project
            var members = await _unitOfWork.Requests
                .GetMemberIdsOfProject(userRequest.ProjectId, cancellationToken);

            if (!members.Any(x => x == userId))
            {
                //sent request for each activity in project
                var userRequests = new List<UserRequest>();
                var activityIds = await _unitOfWork.Activities
                    .GetActiveActivitiesId(userRequest.ProjectId, cancellationToken);

                foreach (var activityId in activityIds)
                {
                    var sendRequest = RequestFactory.CreateActivityRequest(userRequest.ProjectId
                        , activityId, userRequest.SenderId, userRequest.ReceiverId
                        , userRequest.Message, false);

                    sendRequest.MakeUnActive();
                    sendRequest.Accept();
                    userRequests.Add(sendRequest);
                }

                _unitOfWork.Requests.AddRange(userRequests);
            }
            else
            {
                _unitOfWork.Requests.Remove(userRequest);
            }
            userRequest.Accept();
        }
        else if (userRequest.RequestFor == RequestFor.Activity && request.IsAccepted == true)
        {
            userRequest.Accept();
        }
        else
        {
            userRequest.Reject();
        }

        await _unitOfWork.SaveChangeAsync(cancellationToken);
    }
}
