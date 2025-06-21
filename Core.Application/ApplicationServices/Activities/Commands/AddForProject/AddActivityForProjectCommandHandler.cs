using AutoMapper;
using Core.Application.ApplicationServices.Projects.Exceptions;
using Core.Application.Features.Activities.Commands;
using Core.Domain.Entity.Activities;
using Core.Domain.Entity.UserRequests;
using Core.Domain.Interfaces;
using MediatR;

namespace Core.Application.ApplicationServices.Activities.Commands.AddForProject;

public sealed class AddActivityForProjectCommandHandler(IUnitOfWork unitOfWork, ICurrentUserServices currentUser)
    : IRequestHandler<AddActivityForProjectCommandRequest>
{

    public readonly IUnitOfWork _unitOfWork = unitOfWork;
    public readonly ICurrentUserServices _currentUser = currentUser;

    public async Task Handle(AddActivityForProjectCommandRequest request, CancellationToken cancellationToken)
    {
        var ownerId = _currentUser.GetUserId();

        //check if user is the owner of project or not 
        var project = await _unitOfWork.Projects
            .FindById(request.ProjectId, cancellationToken);

        if (project.OwnerId != ownerId)
        {
            throw new OnlyProjectCreatorAllowedException();
        }
        // create
        var activity = ActivityFactory.CreateForProject(ownerId, request.ProjectId
            , request.Title, request.Description
            , request.StartDate,request.Category 
            , request.DurationInMinute
            , request.NotificationBeforeInMinute);

        var userRequests = new List<UserRequest>();
        foreach (var memberId in request.MemberIds)
        {
            var sendRequest = RequestFactory.CreateActivityRequest(activity.ProjectId
                , activity.Id, ownerId, memberId, null, false);

            // make request accepted
            sendRequest.Accept();
            sendRequest.MakeUnActive();

            userRequests.Add(sendRequest);
        }

        // add to activity table
        _unitOfWork.Activities.Add(activity);

        _unitOfWork.Requests.AddRange(userRequests);

        await _unitOfWork.SaveChangeAsync(cancellationToken);
    }
}
