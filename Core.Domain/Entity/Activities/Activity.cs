using Core.Domain.Entity.Comments;
using Core.Domain.Entity.Projects;
using Core.Domain.Entity.UserRequests;
using Core.Domain.Entity.Users;
using Core.Domain.Enum;

namespace Core.Domain.Entity.Activities;

public class Activity
{
    public string Id { get; set; }

    public string? ParentId { get; set; }
    public Activity Parent { get; set; }

    public string UserId { get; set; }
    public User User { get; set; }

    public string ProjectId { get; set; }
    public Project Project { get; set; }

    public string Title { get; set; }

    public string? Description { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime UpdateDate { get; set; }

    public DateTime StartDate { get; set; }

    public TimeSpan? Duration { get; set; }

    public TimeSpan? NotificationBefore { get; set; }

    public ActivityCategory Category { get; set; }

    public bool IsCompleted { get; set; }

    public bool IsEdited { get; set; }

    public List<UserRequest> UserRequests = [];

    public List<Comment> Comments = [];

    public List<Activity> SubActivities = [];

    public static Activity Create(string? parentId, string userId,
        string projectId, string title, string? Description, DateTime startDate
        , int? duration, int? notificationBefore, ActivityCategory category)
    {
        return new Activity
        {
            Id = Guid.NewGuid().ToString(),
            ParentId = parentId,
            UserId = userId,
            ProjectId = projectId,
            Title = title,
            Description = Description,
            Category = category,
            Duration = duration != 0 ? TimeSpan.FromMinutes((double)duration) : null,
            NotificationBefore = notificationBefore != 0 ? TimeSpan.FromMinutes((double)notificationBefore) : null,
            CreatedDate = DateTime.Now,
            UpdateDate = DateTime.Now,
            StartDate = startDate,
            IsCompleted = false,
            IsEdited = false,
        };
    }

    public void MakeComplete()
    {
        IsCompleted = true;
    }
}