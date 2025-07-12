using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class RequestConfiguration : IEntityTypeConfiguration<Request>
{
	public void Configure(EntityTypeBuilder<Request> builder)
	{
		builder.ToTable("Requests");

		builder.Property(x => x.Id)
			.UseIdentityColumn(1000, 1);

		// Activity - UserRequest
		builder.HasOne(r => r.Activity)
			.WithMany(a => a.UserRequests)
			.HasForeignKey(r => r.ActivityId)
			.OnDelete(DeleteBehavior.NoAction);

	}
}
