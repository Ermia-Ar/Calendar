using Core.Domain.Entities.Activities;
using Core.Domain.Entities.Base;
using Core.Domain.Entities.Notifications;

namespace Core.Domain.Entities.ActivityMembers;

public class ActivityMember : BaseEntity
{
	public Guid MemberId { get; set; }

	public long ActivityId { get; set; }
	public Activity Activity { get; set; }

	public long? NotificationId { get; set; }
	public Notification Notification { get; set; }

	public bool IsGuest { get; set; }

	public bool IsOwner { get; set; }

	public static ActivityMember Create(Guid memberId, long activityId, bool isGuest)
	{
		return new ActivityMember
		{
			MemberId = memberId,
			ActivityId = activityId,
			CreatedDate = DateTime.UtcNow,
			NotificationId = null,
			IsActive = true,
			IsGuest = isGuest,
			IsOwner = false, 
		};
	}

	public static ActivityMember CreateOwner(Guid memberId, long activityId)
	{
		return new ActivityMember
		{
			MemberId = memberId,
			ActivityId = activityId,
			CreatedDate = DateTime.UtcNow,
			NotificationId = null,
			IsActive = true,
			IsGuest = false,
			IsOwner = true,
		};
	}

	public void SetNotification(long notificationId)
	{
		NotificationId = notificationId;
	}

	public void RemoveNotification()
	{
		NotificationId = null;
		UpdateDate = DateTime.UtcNow;
	}

	public void MakeGuest()
	{
		IsGuest = true;
		UpdateDate = DateTime.UtcNow;
	}

}
