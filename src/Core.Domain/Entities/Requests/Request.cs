using Core.Domain.Entities.Activities;
using Core.Domain.Entities.Base;
using Core.Domain.Entities.Notifications;
using Core.Domain.Entities.Projects;
using Core.Domain.Enum;

namespace Core.Domain.Entities.Requests;

public class Request : BaseEntity
{
    public Request()
    {
        
    }

    public long ActivityId { get; internal set; }
    public Activity Activity { get; internal set; }

    public Guid SenderId { get; internal set; }

    public Guid ReceiverId { get; internal set; }

    public RequestStatus Status { get; internal set; }

    public DateTime InvitedAt { get; internal set; }

    public DateTime? AnsweredAt { get; internal set; }

    public string? Message { get; internal set; }

    public bool IsExpire { get; internal set; }

    public bool IsGuest { get; internal set; }

	public void Accept()
    {
        Status = RequestStatus.Accepted;
        IsExpire = true;
        AnsweredAt = DateTime.UtcNow;
        UpdateDate = DateTime.UtcNow;
	}

	public void Reject()
    {
        Status = RequestStatus.Rejected;
        IsExpire = true;
        AnsweredAt = DateTime.UtcNow;
        UpdateDate = DateTime.UtcNow;
	}
}