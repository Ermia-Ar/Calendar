using Amazon.S3.Model.Internal.MarshallTransformations;

namespace Core.Domain.Entities.Projects;

public static class ProjectFactory
{
	public static Project Create(string ownerId
		, string title, string description, DateTime startDate, DateTime endDate)
	{
		return new Project
		{
			Id = Guid.NewGuid().ToString(),
			CreatedDate = DateTime.Now,
			UpdateDate = DateTime.Now,
			Description = description,
			Title = title,
			EndDate = endDate,
			StartDate = startDate,
			OwnerId = ownerId,
			IsActive = true,
		};
	}

	public static Project GetDefault()
	{
		return new Project
		{
			Id = "8c56ac14-ae28-4425-9a19-690d27d3a16d",
			OwnerId = "05e404b3-e235-4c11-bff4-3754b22c0245",
			Title = "Public Project",
			StartDate = DateTime.Now,
			EndDate = DateTime.MaxValue,
			CreatedDate = DateTime.Now,
			Description = "this is static project",
			UpdateDate = DateTime.Now,
		};
	}
}
