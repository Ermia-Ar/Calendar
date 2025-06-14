using Core.Application.ApplicationServices.UserRequests.Exceptions;
using Core.Domain.Entity;
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
        //var userId = _currentUserServices.GetUserId();
        ////get request
        //var userRequest = await _unitOfWork.Requests
        //    .FindById(request.RequestId, cancellationToken);

        //if (userRequest.ReceiverId != userId)
        //{
        //    throw new NotFoundRequestException();
        //}
        //if (userRequest.IsExpire == true)
        //{
        //    throw new ExpireRequestException();
        //}

        ////check if the request for a Project 
        //if (userRequest.RequestFor == RequestFor.Project && request.IsAccepted == true)
        //{
        //    // if the user is already member of project 
        //    var members = (await _unitOfWork.Requests
        //        .GetMemberOfProject(userRequest.ProjectId, cancellationToken))
        //        .Adapt<List<User>>();

        //    if (!members.Any(x => x.Id == userId))
        //    {
        //        //sent request for each activity in project
        //        var userRequests = new List<UserRequest>();
        //        var activityIds = await _unitOfWork.Activities
        //            .GetActiveActivitiesId(userRequest.ProjectId, cancellationToken);

        //        foreach (var activityId in activityIds)
        //        {
        //            var sendRequest = UserRequest.CreateUserRequest(activityId
        //                , userRequest.ProjectId, userRequest.SenderId
        //                , userRequest.ReceiverId, null
        //                , false, RequestStatus.Accepted);

        //            userRequests.Add(sendRequest);
        //        }
        //        await _unitOfWork.Requests.AddRangeRequest(userRequests, cancellationToken);
        //        //AnswerRequest
        //        userRequest.IsExpire = true;
        //        userRequest.Status = RequestStatus.Accepted;
        //        userRequest.AnsweredAt = DateTime.Now;
        //        //update request
        //        _unitOfWork.Requests.UpdateRequest(userRequest);
        //    }
        //    else
        //    {
        //        _unitOfWork.Requests.Remove(userRequest);
        //    }
        //}
        //else if (userRequest.RequestFor == RequestFor.Activity && request.IsAccepted == true)
        //{
        //    // if the user is already member of Activity
        //    var members = (await _unitOfWork.Requests
        //        .GetMemberOfActivity(userRequest.ActivityId, cancellationToken))
        //        .Adapt<List<User>>();

        //    if (members.Any(x => x.Id == userId))
        //    {
        //        _unitOfWork.Requests.Remove(userRequest);
        //    }
        //    else
        //    {
        //        //AnswerRequest
        //        userRequest.IsExpire = true;
        //        userRequest.Status = RequestStatus.Accepted;
        //        userRequest.AnsweredAt = DateTime.Now;
        //        //update request
        //        _unitOfWork.Requests.UpdateRequest(userRequest);
        //    }
        //}
        //else
        //{
        //    //AnswerRequest
        //    userRequest.IsExpire = true;
        //    userRequest.Status = RequestStatus.Rejected;
        //    userRequest.AnsweredAt = DateTime.Now;
        //    //update request
        //    _unitOfWork.Requests.UpdateRequest(userRequest);
        //}
        throw new NotImplementedException();

        //await _unitOfWork.SaveChangeAsync(cancellationToken);
    }
}
