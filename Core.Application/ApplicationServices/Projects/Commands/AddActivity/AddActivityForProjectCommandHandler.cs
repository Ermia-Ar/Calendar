using Core.Application.ApplicationServices.Auth.Exceptions;
using Core.Application.ApplicationServices.Projects.Exceptions;
using Core.Application.ApplicationServices.Requests.Queries.GetAll;
using Core.Application.Common;
using Core.Domain.Entities.Activities;
using Core.Domain.Entities.Notifications;
using Core.Domain.Entities.Requests;
using Core.Domain.UnitOfWork;
using Mapster;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Core.Application.ApplicationServices.Projects.Commands.AddActivity;

public sealed class AddActivityForProjectCommandHandler(
    IUnitOfWork unitOfWork,
    ICurrentUserServices currentUser,
    IConfiguration configuration
    ) : IRequestHandler<AddActivityForProjectCommandRequest, Dictionary<string, GetAllRequestQueryResponse>>
{

    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICurrentUserServices _currentUser = currentUser;
    private readonly IConfiguration _configuration = configuration;


    public async Task<Dictionary<string, GetAllRequestQueryResponse>> Handle(AddActivityForProjectCommandRequest request, CancellationToken cancellationToken)
    {
        var ownerId = _currentUser.GetUserId();


        //check if user is the member of project or not 
        var memberIds = await _unitOfWork.Requests
            .FindMemberIdsOfProject(request.ProjectId, cancellationToken);

        if (memberIds.Any(x => x == ownerId))
        {
            throw new OnlyProjectMembersAllowedException();
        }

        // create activity for project
        var activity = ActivityFactory.CreateForProject(ownerId, request.ProjectId
            , request.Title, request.Description
            , request.StartDate, request.Category
            , request.Duration);

        // for signalR 
        var response = new Dictionary<string, GetAllRequestQueryResponse>();
        //
        var userRequests = new List<Request>();
        foreach (var memberId in request.MemberIds)
        {
            //check
            var member = await _unitOfWork.Users.FindById(memberId);
            if (member == null)
            {
                throw new NotFoundUserIdException(memberId);
            }
            // create request for memberId
            var sendRequest = RequestFactory.CreateActivityRequest(activity.ProjectId
                , activity.Id, ownerId, memberId, null, false);

            userRequests.Add(sendRequest);
            //for signalR
            response[memberId] = sendRequest.Adapt<GetAllRequestQueryResponse>();
        }

        //add owner to activity members
        var ownerRequest = RequestFactory.CreateActivityRequest(activity.ProjectId
            , activity.Id, ownerId, ownerId
            , null, false);

        ownerRequest.Accept();
        ownerRequest.MakeArchived();

        // create notification for owner
        // if NotificationBefore is null set default notification
        var notificationBefore = request.NotificationBefore ??
            TimeSpan.FromHours(double.Parse(_configuration["Public:DefaultNotification"]));

        var notification = NotificationFactory
            .Create(ownerRequest.Id
                , activity.StartDate - notificationBefore);

        ownerRequest.SetNotification(notification.Id);

        userRequests.Add(ownerRequest);

        // add to activity table
        _unitOfWork.Activities.Add(activity);
        //add requests
        _unitOfWork.Requests.AddRange(userRequests);
        //add notifications
        _unitOfWork.Notifications.Add(notification);

        await _unitOfWork.SaveChangeAsync(cancellationToken);

        return response;
    }
}
