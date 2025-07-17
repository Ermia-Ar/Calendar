using Core.Domain.Entities.ActivityMembers;
using Core.Domain.Entities.ProjectMembers;

namespace Infrastructure.Persistence.Configurations;

public static class SoftDeleteConfiguration
{
	public static void ConfigSoftDeleteFilter(this ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Activity>().HasQueryFilter(f => f.IsActive);
		modelBuilder.Entity<Project>().HasQueryFilter(f => f.IsActive);
		modelBuilder.Entity<Notification>().HasQueryFilter(f => f.IsActive);
		modelBuilder.Entity<ActivityRequest>().HasQueryFilter(f => f.IsActive);
		modelBuilder.Entity<Comment>().HasQueryFilter(f => f.IsActive);
		modelBuilder.Entity<ActivityMember>().HasQueryFilter(x => x.IsActive);
		modelBuilder.Entity<ProjectMember>().HasQueryFilter(x => x.IsActive);
		
	}
}