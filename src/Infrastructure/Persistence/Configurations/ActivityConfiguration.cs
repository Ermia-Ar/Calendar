using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class ActivityConfiguration : IEntityTypeConfiguration<Activity>
{
	public void Configure(EntityTypeBuilder<Activity> builder)
	{
		builder.ToTable("Activities");

		builder.Property(x => x.Id)
			.UseIdentityColumn(1000, 1);

		// Project - Activity
		builder.HasOne<Project>()
			.WithMany(p => p.Activities)
			.HasForeignKey(a => a.ProjectId)
			.OnDelete(DeleteBehavior.NoAction);

		// Activity - SubActivities (Self-referencing)
		builder.HasOne<Activity>()
			.WithMany(a => a.SubActivities)
			.HasForeignKey(a => a.ParentId)
			.OnDelete(DeleteBehavior.NoAction);
	}
}