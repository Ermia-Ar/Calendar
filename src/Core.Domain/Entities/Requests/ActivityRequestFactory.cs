namespace Core.Domain.Entities.Requests;

public static class ActivityRequestFactory
{
	public static ActivityRequest Create(Guid receiverId,
		long activityId, string massage)
	{
		return new ActivityRequest
		{
			ActivityId = activityId,
			ReceiverId = receiverId,
			InvitedAt = DateTime.UtcNow,
			Message = massage,
			CreatedDate = DateTime.UtcNow,
			IsActive = true
		};
	}
}