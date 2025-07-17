using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class ActivityRequestConfiguration : IEntityTypeConfiguration<ActivityRequest>
{
	public void Configure(EntityTypeBuilder<ActivityRequest> builder)
	{
		builder.ToTable("ActivityRequests");

		builder.Property(x => x.Id)
			.UseIdentityColumn(1000);

		// Activity - ActivityRequest
		builder.HasOne<Activity>()
			.WithMany(a => a.UserRequests)
			.HasForeignKey(r => r.ActivityId)
			.OnDelete(DeleteBehavior.NoAction);

	}
}