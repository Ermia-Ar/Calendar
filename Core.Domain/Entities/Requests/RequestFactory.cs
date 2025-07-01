using Core.Domain.Entities.Requests;
using Core.Domain.Enum;

public static class RequestFactory
{
    public static Request CreateProjectRequest(string projectId
  , string senderId, string receiverId, string? massage)
    {
        return new Request
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
			CreatedDate = DateTime.Now,
			UpdateDate = DateTime.Now,
			IsExpire = false,
            IsArchived = true,
            IsGuest = false,
			IsActive = true,
        };
	}

    public static Request CreateActivityRequest(string projectId, string activityId
        , string senderId, string receiverId, string? massage, bool isGuest)
    {
        return new Request
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
			CreatedDate = DateTime.Now,
			UpdateDate = DateTime.Now,
			IsExpire = false,
            IsArchived = true,
            IsGuest = isGuest,
			IsActive = true,
        };
	}
}
