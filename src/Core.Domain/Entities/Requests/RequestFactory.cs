using Core.Domain.Entities.Requests;
using Core.Domain.Enum;

public static class RequestFactory
{
    public static Request Create(long activityId
		, Guid senderId, Guid receiverId, string? massage, bool isGuest)
    {
	    if (senderId == receiverId)
		    throw new Exception();
	    
        return new Request
        {
            ActivityId = activityId,
            SenderId = senderId,
            ReceiverId = receiverId,
            InvitedAt = DateTime.UtcNow,
            AnsweredAt = null,
            Status = RequestStatus.Pending,
            Message = massage,
			CreatedDate = DateTime.UtcNow,
			IsExpire = false,
			IsGuest = isGuest,
            IsActive = true
		};
	}
}
