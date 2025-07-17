using Core.Domain.Entities.ActivityMembers;
using Core.Domain.Entities.Base;
using Core.Domain.Entities.Requests;

namespace Core.Domain.Entities.Notifications;

public class Notification : BaseEntity
{
	public Guid UserId { get; set; }
	
	public long ActivityId { get; internal set; }
	
	public DateTime NotificationDate { get; internal set; }
	

	public void UpdateNotification(DateTime newNotification)
	{
		NotificationDate = newNotification;
		UpdateDate = DateTime.UtcNow;
	}

}

