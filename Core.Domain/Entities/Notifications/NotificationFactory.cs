
namespace Core.Domain.Entities.Notifications;

public static class NotificationFactory
{
	public static Notification Create(string RequestId, DateTime NotificationDate)
	{
		return new Notification
		{
			Id = Guid.NewGuid().ToString(),
			RequestId = RequestId,
			NotificationDate = NotificationDate,
			CreatedDate = DateTime.Now,
			UpdateDate = DateTime.Now,
			IsSent = false,
			IsActive = true,
		};
	}
}

