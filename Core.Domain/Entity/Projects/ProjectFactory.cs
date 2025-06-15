namespace Core.Domain.Entity.Projects;

public static class ProjectFactory
{
    public static Project Create(string ownerId
        , string title, string description, DateTime startDate, DateTime endDate)
    {
        return new Project
        {
            Id = Guid.NewGuid().ToString(),
            CreatedDate = DateTime.Now,
            UpdateDate = endDate,
            Description = description,
            Title = title,
            EndDate = endDate,
            StartDate = startDate,
            OwnerId = ownerId,
        };
    }
}
