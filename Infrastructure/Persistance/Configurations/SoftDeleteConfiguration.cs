using Core.Domain.Entities.Activities;
using Core.Domain.Entities.Comments;
using Core.Domain.Entities.Notifications;
using Core.Domain.Entities.Projects;
using Core.Domain.Entities.Requests;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Configurations;

public static class SoftDeleteConfiguration
{
	public static void ConfigSoftDeleteFilter(this ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Activity>().HasQueryFilter(f => f.IsActive);
		modelBuilder.Entity<Project>().HasQueryFilter(f => f.IsActive);
		modelBuilder.Entity<Notification>().HasQueryFilter(f => f.IsActive);
		modelBuilder.Entity<Request>().HasQueryFilter(f => f.IsActive);
		modelBuilder.Entity<Comment>().HasQueryFilter(f => f.IsActive);
	}
}