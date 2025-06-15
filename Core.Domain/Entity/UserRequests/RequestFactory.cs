using Core.Domain.Entity.UserRequests;
using Core.Domain.Enum;
using System.Runtime.CompilerServices;

public static class RequestFactory
{
    public static UserRequest CreateProjectRequest(string projectId
  , string senderId, string receiverId, string? massage)
    {
        return new UserRequest
        {
            Id = Guid.NewGuid().ToString(),
            ActivityId = null,
            ProjectId = projectId,
            SenderId = senderId,
            ReceiverId = receiverId,
            InvitedAt = DateTime.Now,
            AnsweredAt = null,
            RequestFor = RequestFor.Project,
            Status = RequestStatus.Pending,
            Message = massage,
            IsExpire = false,
            IsActive = true,
            IsGuest = false,
        };
    }

    public static UserRequest CreateActivityRequest(string projectId, string activityId
        , string senderId, string receiverId, string? massage, bool isGuest)
    {
        return new UserRequest
        {
            Id = Guid.NewGuid().ToString(),
            ActivityId = activityId,
            ProjectId = projectId,
            SenderId = senderId,
            ReceiverId = receiverId,
            InvitedAt = DateTime.Now,
            AnsweredAt = null,
            RequestFor = RequestFor.Activity,
            Status = RequestStatus.Pending,
            Message = massage,
            IsExpire = false,
            IsActive = true,
            IsGuest = isGuest
        };
    }
}
