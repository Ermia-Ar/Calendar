using Amazon.S3.Model.Internal.MarshallTransformations;

namespace Core.Domain.Entities.Projects;

public static class ProjectFactory
{
	public static Project Create(Guid ownerId, string title, 
		string description, DateTime startDate, DateTime endDate,
		string icon, string color)
	{
		return new Project
		{
			CreatedDate = DateTime.UtcNow,
			Description = description,
			Title = title,
			EndDate = endDate,
			StartDate = startDate,
			OwnerId = ownerId,
			IsActive = true,
			Color = color,
			Icon = icon
		};
	}

}
