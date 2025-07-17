
namespace Core.Domain.Entities.Notifications;

public static class NotificationFactory
{
	public static Notification Create(Guid userId, long activityId, DateTime notificationDate)
	{
		return new Notification
		{
			UserId = userId,
			ActivityId = activityId,
			NotificationDate = notificationDate,
			CreatedDate = DateTime.UtcNow,
			IsActive = true,
		};
	}
}

