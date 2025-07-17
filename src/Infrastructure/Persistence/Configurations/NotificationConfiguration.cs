using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
	public void Configure(EntityTypeBuilder<Notification> builder)
	{

		builder.Property(x => x.Id)
			.UseIdentityColumn(1000, 1);

		builder.HasOne<Activity>()
			.WithMany(x => x.Notifications)
			.HasForeignKey(x => x.ActivityId)
			.OnDelete(DeleteBehavior.NoAction);
	}
}
