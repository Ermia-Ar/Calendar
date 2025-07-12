
namespace Core.Domain.Entities.Notifications;

public static class NotificationFactory
{
	public static Notification Create(long ActivityMemberId, DateTime NotificationDate)
	{
		return new Notification
		{
			ActivityMemberId = ActivityMemberId,
			NotificationDate = NotificationDate,
			CreatedDate = DateTime.UtcNow,
			IsSent = false,
			IsActive = true,
		};
	}
}

