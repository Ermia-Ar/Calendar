using Core.Domain.Entities.Activities;
using Core.Domain.Entities.Base;
using Core.Domain.Entities.Notifications;
using Core.Domain.Entities.Projects;
using Core.Domain.Entities.Users;
using Core.Domain.Enum;

namespace Core.Domain.Entities.Requests;

public class Request : BaseEntity
{
    public Request()
    {
        
    }

    public string? ActivityId { get; internal set; }
    public Activity Activity { get; internal set; }

    public string ProjectId { get; internal set; }
    public Project Project { get; internal set; }

    public string SenderId { get; internal set; }
    public User Sender { get; internal set; }

    public string ReceiverId { get; internal set; }
    public User Receiver { get; internal set; }

    public string? NotificationId { get; internal set; }
    public Notification? Notification { get; internal set; }

    public RequestFor RequestFor { get; internal set; }

    public RequestStatus Status { get; internal set; }

    public DateTime InvitedAt { get; internal set; }

    public DateTime? AnsweredAt { get; internal set; }

    public string? Message { get; internal set; }

    public bool IsExpire { get; internal set; }

    public bool IsArchived { get; internal set; }

    public bool IsGuest { get; internal set; }

    public void MakeArchived()
    {
        IsArchived = false;
    }

	public void Accept()
    {
        Status = RequestStatus.Accepted;
        IsExpire = true;
        AnsweredAt = DateTime.Now;
        UpdateDate = DateTime.Now;
        IsEdited = true;
	}

	public void Reject()
    {
        Status = RequestStatus.Rejected;
        IsExpire = true;
        AnsweredAt = DateTime.Now;
        UpdateDate = DateTime.Now;
        IsEdited = true;
	}

	public void SetNotification(string notificationId)
    {
        NotificationId = notificationId;
        UpdateDate = DateTime.Now;
        IsEdited = true;
    }
}