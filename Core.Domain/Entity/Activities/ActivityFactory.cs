using Core.Domain.Enum;

namespace Core.Domain.Entity.Activities;

public static class ActivityFactory
{
    public static Activity Create(string userId
        , string title, string? Description, DateTime startDate, ActivityCategory category
        , int duration = 0, int notificationBefore = 0)
    {
        return new Activity()
        {
            Id = Guid.NewGuid().ToString(),
            ParentId = null,
            UserId = userId,
            ProjectId = "8c56ac14-ae28-4425-9a19-690d27d3a16d",
            Title = title,
            Description = Description,
            Category = category,
            Duration = duration != 0 ? TimeSpan.FromMinutes(duration) : null,
            NotificationBefore = notificationBefore != 0 ? TimeSpan.FromMinutes(notificationBefore) : null,
            CreatedDate = DateTime.Now,
            UpdateDate = DateTime.Now,
            StartDate = startDate,
            IsCompleted = false,
            IsEdited = false,
        };
    }
    public static Activity CreateForProject(string userId,string projectId
        , string title, string? Description, DateTime startDate, ActivityCategory category
        , int duration = 0, int notificationBefore = 0)
    {
        return new Activity()
        {
            Id = Guid.NewGuid().ToString(),
            ParentId = null,
            UserId = userId,
            ProjectId = projectId,
            Title = title,
            Description = Description,
            Category = category,
            Duration = duration != 0 ? TimeSpan.FromMinutes(duration) : null,
            NotificationBefore = notificationBefore != 0 ? TimeSpan.FromMinutes(notificationBefore) : null,
            CreatedDate = DateTime.Now,
            UpdateDate = DateTime.Now,
            StartDate = startDate,
            IsCompleted = false,
            IsEdited = false,
        };
    }
    public static Activity CreateSubActivity(string parentId, string userId,string projectId
        , string title, string? Description, DateTime startDate, ActivityCategory category
        , int duration = 0, int notificationBefore = 0)
    {
        return new Activity()
        {
            Id = Guid.NewGuid().ToString(),
            ParentId = parentId,
            UserId = userId,
            ProjectId = projectId,
            Title = title,
            Description = Description,
            Category = category,
            Duration = duration != 0 ? TimeSpan.FromMinutes(duration) : null,
            NotificationBefore = notificationBefore != 0 ? TimeSpan.FromMinutes(notificationBefore) : null,
            CreatedDate = DateTime.Now,
            UpdateDate = DateTime.Now,
            StartDate = startDate,
            IsCompleted = false,
            IsEdited = false,
        };
    }
}