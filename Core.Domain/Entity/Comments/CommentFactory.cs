namespace Core.Domain.Entity.Comments;

public static class CommentFactory
{
    public static Comment Create(string userId
        , string activityId, string projectId, string content)
    {
        return new Comment()
        {
            Id = Guid.NewGuid().ToString(),
            ActivityId = activityId,
            ProjectId = projectId,
            UserId = userId,
            Content = content,
            CreatedDate = DateTime.Now,
            UpdatedDate = DateTime.Now,
            IsEdited = false,
        };
    }
}
