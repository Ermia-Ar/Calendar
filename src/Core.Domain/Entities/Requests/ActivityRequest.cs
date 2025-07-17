using Core.Domain.Entities.Activities;
using Core.Domain.Entities.Base;

namespace Core.Domain.Entities.Requests;

public class ActivityRequest : BaseEntity
{
    public long ActivityId { get; internal set; }

    public Guid ReceiverId { get; internal set; }

    public DateTime InvitedAt { get; internal set; }

    public string Message { get; internal set; }

    
}