using Core.Domain.Entities.ActivityMembers;
using Core.Domain.Entities.Base;
using Core.Domain.Entities.Requests;

namespace Core.Domain.Entities.Notifications;

public class Notification : BaseEntity
{
	public long ActivityMemberId { get; internal set; }
	
	public ActivityMember ActivityMember { get; internal set; }

	public DateTime NotificationDate { get; internal set; }

	public bool IsSent { get; internal set; }


	public void Sent()
	{
		IsSent = true;
		UpdateDate = DateTime.UtcNow;
	}

	public void UpdateNotification(DateTime newNotification)
	{
		NotificationDate = newNotification;
		UpdateDate = DateTime.UtcNow;
	}

}

