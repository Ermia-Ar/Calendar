using Core.Domain.Enum;

namespace Core.Domain.Entities.Activities;
public static class ActivityFactory
{
	public static Activity Create(Guid userId, long projectId
		, string title, string? Description, DateTime startDate, ActivityCategory category
		, TimeSpan? duration)
	{
		return new Activity()
		{
			ParentId = null,
			UserId = userId,
			ProjectId = projectId,
			Title = title,
			Description = Description,
			Category = category,
			Duration = duration,
			CreatedDate = DateTime.UtcNow,
			StartDate = startDate,
			IsCompleted = false,
			IsActive = true,
		};
	}
	public static Activity CreateSubActivity(long? parentId, Guid userId, long projectId
		, string title, string? Description, DateTime startDate, ActivityCategory category
		, TimeSpan? duration)
	{
		return new Activity()
		{
			ParentId = parentId,
			UserId = userId,
			ProjectId = projectId,
			Title = title,
			Description = Description,
			Category = category,
			Duration = duration,
			CreatedDate = DateTime.UtcNow,
			StartDate = startDate,
			IsCompleted = false,
			IsActive = true,
		};
	}
}