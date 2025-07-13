using Core.Domain.Entities.ActivityMembers;
using Core.Domain.Entities.Base;
using Core.Domain.Entities.Comments;
using Core.Domain.Entities.Projects;
using Core.Domain.Entities.Requests;
using Core.Domain.Enum;

namespace Core.Domain.Entities.Activities;
 
public class Activity : BaseEntity
{
    public long? ParentId { get; internal set; }
    public Activity Parent { get; internal set; }

    public Guid UserId { get; internal set; }

    public long ProjectId { get; internal set; }
    public Project Project { get; internal set; }   

    public string Title { get; internal set; }

    public string? Description { get; internal set; }

    public DateTime StartDate { get; internal set; }

    public TimeSpan? Duration { get; internal set; }

    public ActivityCategory Category { get; internal set; }

    public bool IsCompleted { get; internal set; }


    public List<ActivityMember> Members = [];

    public List<Request> UserRequests = [];

    public List<Comment> Comments = [];

    public List<Activity> SubActivities = [];


    public void Update(string title, string? description,
        TimeSpan? duration, ActivityCategory category)
    {
        Title = title;
        Description = description;
        Duration = duration;
        Category = category;
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