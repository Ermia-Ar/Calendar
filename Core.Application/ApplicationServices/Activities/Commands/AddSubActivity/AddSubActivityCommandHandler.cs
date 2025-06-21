using AutoMapper;
using Core.Application.ApplicationServices.Activities.Exceptions;
using Core.Domain.Entity.Activities;
using Core.Domain.Entity.UserRequests;
using Core.Domain.Interfaces;
using Mapster;
using MediatR;

namespace Core.Application.ApplicationServices.Activities.Commands.AddSubActivity;

public sealed class AddSubActivityCommandHandler(
    IUnitOfWork unitOfWork, ICurrentUserServices currentUser, IMapper mapper)
            : IRequestHandler<AddSubActivityCommandRequest>
{
    public readonly IUnitOfWork _unitOfWork = unitOfWork;
    public readonly ICurrentUserServices _currentUser = currentUser;
    public readonly IMapper _mapper = mapper;

    public async Task Handle(AddSubActivityCommandRequest request, CancellationToken cancellationToken)
    {
        var ownerId = _currentUser.GetUserId();

        //get base activity
        var baseActivity = await _unitOfWork.Activities
            .FindById(request.ActivityId, cancellationToken);

        if (baseActivity.UserId != ownerId)
        {
            throw new OnlyActivityCreatorAllowedException();
        }

        // create sub activity
        var subActivity = ActivityFactory.CreateSubActivity(baseActivity.Id, ownerId, baseActivity.ProjectId
            , baseActivity.Title, request.Description
            , request.StartDate, baseActivity.Category
            , request.DurationInMinute
            , request.NotificationBeforeInMinute);


        //create request for all members of base activity
        var userRequests = new List<UserRequest>();
        foreach (var memberId in request.MemberIds)
        {
            var sendRequest = RequestFactory.CreateActivityRequest
                (subActivity.ProjectId, subActivity.Id
                , ownerId, memberId, null, false);
            // make request accepted
            sendRequest.MakeUnActive();
            sendRequest.Accept();

            userRequests.Add(sendRequest);
        }

        //add to table activity
        _unitOfWork.Activities.Add(subActivity);
        //send all requests
        _unitOfWork.Requests.AddRange(userRequests);

        await _unitOfWork.SaveChangeAsync(cancellationToken);
    }
}
