namespace Core.Domain.Entities.Comments;

public static class CommentFactory
{
    public static Comment Create(Guid userId
        , long activityId, long projectId, string content)
    {
        return new Comment()
        {
            ActivityId = activityId,
            ProjectId = projectId,
            UserId = userId,
            Content = content,
			CreatedDate = DateTime.UtcNow,
			IsActive = true,
        };
	}
}
