using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
	public void Configure(EntityTypeBuilder<Notification> builder)
	{

		builder.Property(x => x.Id)
			.UseIdentityColumn(1000, 1);

		builder.HasOne(x => x.ActivityMember)
			.WithOne(x => x.Notification)
			.HasForeignKey<Notification>(x => x.ActivityMemberId)
			.OnDelete(DeleteBehavior.NoAction);
	}
}
