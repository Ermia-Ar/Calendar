using Core.Domain.Entities.Base;
using Core.Domain.Entities.Comments;
using Core.Domain.Entities.Projects;
using Core.Domain.Entities.Requests;
using Core.Domain.Entities.Users;
using Core.Domain.Enum;

namespace Core.Domain.Entities.Activities;
 
public class Activity : BaseEntity
{
    public string? ParentId { get; internal set; }
    public Activity Parent { get; internal set; }

    public string UserId { get; internal set; }
    public User User { get; internal set; }

    public string ProjectId { get; internal set; }
    public Project Project { get; internal set; }

    public string Title { get; internal set; }

    public string? Description { get; internal set; }

    public DateTime StartDate { get; internal set; }

    public TimeSpan? Duration { get; internal set; }

    public ActivityCategory Category { get; internal set; }

    public bool IsCompleted { get; internal set; }


    public List<Request> UserRequests = [];

    public List<Comment> Comments = [];

    public List<Activity> SubActivities = [];

    public void MakeComplete()
    {
        IsCompleted = true;
        IsEdited = true;
        UpdateDate = DateTime.Now;
    }

    public void ChangeStartDate(DateTime NewDate)
    {
        StartDate = NewDate;
        IsEdited = true;
        UpdateDate = DateTime.Now;
        
    }
}