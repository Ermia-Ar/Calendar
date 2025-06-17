using Core.Domain.Entity.Activities;
using Core.Domain.Entity.Projects;
using Core.Domain.Entity.Users;
using Core.Domain.Enum;

namespace Core.Domain.Entity.UserRequests;

public class UserRequest
{
    public UserRequest()
    {
        
    }
    public string Id { get; set; }

    public string? ActivityId { get; set; }
    public Activity Activity { get; set; }

    public string ProjectId { get; set; }
    public Project Project { get; set; }

    public string SenderId { get; set; }
    public User Sender { get; set; }

    public string ReceiverId { get; set; }
    public User Receiver { get; set; }

    public RequestFor RequestFor { get; set; }

    public RequestStatus Status { get; set; }

    public DateTime InvitedAt { get; set; }

    public DateTime? AnsweredAt { get; set; }

    public string? Message { get; set; }

    public bool IsExpire { get; set; }

    public bool IsActive { get; set; }
    //TODO :
    public bool IsGuest { get; set; }

    public void MakeUnActive()
    {
        IsActive = false;
    }

    public void Accept()
    {
        Status = RequestStatus.Accepted;
        IsExpire = true;
        AnsweredAt = DateTime.Now;
    }

    public void Reject()
    {
        Status = RequestStatus.Rejected;
        IsExpire = true;
        AnsweredAt = DateTime.Now;
    }
}