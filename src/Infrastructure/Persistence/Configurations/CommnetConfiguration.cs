using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class CommnetConfiguration : IEntityTypeConfiguration<Comment>
{
	public void Configure(EntityTypeBuilder<Comment> builder)
	{

		builder.ToTable("Comments");

		builder.Property(x => x.Id)
			.UseIdentityColumn(1000, 1);

		// Activity - Comment
		builder.HasOne<Activity>()
			.WithMany(a => a.Comments)
			.HasForeignKey(c => c.ActivityId)
			.OnDelete(DeleteBehavior.NoAction);
	}
}
