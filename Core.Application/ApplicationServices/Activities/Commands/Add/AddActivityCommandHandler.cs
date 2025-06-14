using Core.Application.ApplicationServices.Auth.Exceptions;
using Core.Domain.Entity;
using Core.Domain.Enum;
using Core.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Core.Application.ApplicationServices.Activities.Commands.Add;

public sealed class AddActivityCommandHandler(
    IUnitOfWork unitOfWork,
    ICurrentUserServices currentUser,
    IConfiguration configuration
) : IRequestHandler<AddActivityCommandRequest>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICurrentUserServices _currentUser = currentUser;
    private readonly IConfiguration _configuration = configuration;

    public async Task Handle(AddActivityCommandRequest request, CancellationToken cancellationToken)
    {
        string projectId = "8c56ac14-ae28-4425-9a19-690d27d3a16d";
        var ownerId = _currentUser.GetUserId();

        // map to activity table
        var activity = Activity.Create(null, ownerId, projectId,
            request.Title, request.Description,
            request.StartDate, request.DurationInMinute,
            request.NotificationBeforeInMinute,
            request.Category);


        //create request for all members
        var userRequests = new List<UserRequest>();
        foreach (var memberName in request.Members)
        {
            //check
            var member = await _unitOfWork.Users.FindById(memberName);
            if (member == null)
            {
                throw new NotFoundUserNameException(memberName);
            }
            var sendRequest1 = UserRequest.CreateUserRequest(activity.Id
                , projectId, ownerId, member.Id
                , request.Message
                , false, RequestStatus.Pending);

            userRequests.Add(sendRequest1);
        }

        //add owner to activity members
        var sendRequest = UserRequest.CreateUserRequest(activity.Id
                , activity.ProjectId, ownerId, ownerId
                , request.Message
                , false, RequestStatus.Accepted);
        sendRequest.IsActive = false;
        userRequests.Add(sendRequest);


        //add to activity table
        _unitOfWork.Activities.Add(activity);
        //send all requests
        _unitOfWork.Requests.AddRange(userRequests);

        await _unitOfWork.SaveChangeAsync(cancellationToken);
    }
}