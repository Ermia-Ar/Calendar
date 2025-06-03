using Core.Application.Exceptions.User;
using Core.Domain.Entity;
using Core.Domain;
using Core.Domain.Enum;
using MediatR;

namespace Core.Application.ApplicationServices.Activities.Commands.Add;

public sealed class AddActivityCommandHandler(
    IUnitOfWork unitOfWork,
    ICurrentUserServices currentUser
) : IRequestHandler<AddActivityCommandRequest>
{
    public readonly IUnitOfWork _unitOfWork = unitOfWork;
    public readonly ICurrentUserServices _currentUser = currentUser;


    public async Task Handle(AddActivityCommandRequest request, CancellationToken cancellationToken)
    {
        string projectId = "8c56ac14-ae28-4425-9a19-690d27d3a16d";
        var ownerId = _currentUser.GetUserId();
        var ownerName = _currentUser.GetUserName();

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
            var member = await _unitOfWork.Users.FindByUserName(memberName);
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
        await _unitOfWork.Activities.AddActivity(activity, cancellationToken);
        //send all requests
        await _unitOfWork.Requests.AddRangeRequest(userRequests, cancellationToken);

        await _unitOfWork.SaveChangeAsync(cancellationToken);
    }
}