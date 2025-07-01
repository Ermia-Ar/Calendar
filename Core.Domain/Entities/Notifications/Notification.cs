using Core.Domain.Entities.Base;
using Core.Domain.Entities.Requests;

namespace Core.Domain.Entities.Notifications;

public class Notification : BaseEntity
{
	public string RequestId { get; internal set; }
	public Request Request { get; internal set; }
	public DateTime NotificationDate { get; internal set; }
	public bool IsSent { get; internal set; }

	public void MakeUnActive()
	{
		IsActive = false;
	}

	public void sent()
	{
		IsSent = true;
		IsEdited = true;
		UpdateDate = DateTime.Now;
	}

	public void UpdateNotification(DateTime NewNotification)
	{
		NotificationDate = NewNotification;
		IsEdited = true;
		UpdateDate = DateTime.Now;
	}

}

