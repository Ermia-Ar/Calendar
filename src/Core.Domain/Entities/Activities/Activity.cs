using Core.Domain.Entities.ActivityMembers;
using Core.Domain.Entities.Base;
using Core.Domain.Entities.Comments;
using Core.Domain.Entities.Notifications;
using Core.Domain.Entities.Projects;
using Core.Domain.Entities.Requests;
using Core.Domain.Enum;

namespace Core.Domain.Entities.Activities;
 
public class Activity : BaseEntity
{
    public long? ParentId { get; internal set; }
    
    public Guid OwnerId { get; internal set; }

    public long ProjectId { get; internal set; }

    public string Title { get; internal set; } 

    public string? Description { get; internal set; }

    public DateTime StartDate { get; internal set; }

    public TimeSpan? Duration { get; internal set; }

    public ActivityType Type { get; internal set; }

    public bool IsCompleted { get; internal set; }


    public List<ActivityMember> Members = [];

    public List<ActivityRequest> UserRequests = [];

    public List<Comment> Comments = [];

    public List<Activity> SubActivities = [];
    
    public List<Notification> Notifications = [];


    public void Update(string title, string? description,
        TimeSpan? duration, ActivityType type)
    {
        Title = title;
        Description = description;
        Duration = duration;
        Type = type;
        UpdateDate = DateTime.UtcNow;
    }
    public void MakeComplete()
    {
        IsCompleted = true;
        UpdateDate = DateTime.UtcNow;
    }

    public void ChangeStartDate(DateTime newDate)
    {
        StartDate = newDate;
        UpdateDate = DateTime.UtcNow;
    }
}