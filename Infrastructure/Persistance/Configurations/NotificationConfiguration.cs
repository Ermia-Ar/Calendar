using Core.Domain.Entities.Notifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistance.Configurations;

public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
	public void Configure(EntityTypeBuilder<Notification> builder)
	{

		// notification - request 
		builder.HasOne(x => x.Request)
			.WithOne(x => x.Notification)
			.HasForeignKey<Notification>(x => x.RequestId)
			.OnDelete(DeleteBehavior.NoAction);
	}
}
