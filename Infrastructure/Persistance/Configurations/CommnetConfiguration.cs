using Core.Domain.Entities.Comments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistance.Configurations;

public class CommnetConfiguration : IEntityTypeConfiguration<Comment>
{
	public void Configure(EntityTypeBuilder<Comment> builder)
	{

		builder.ToTable("Comments");


		// Project - Comment
		builder.HasOne(c => c.Project)
			.WithMany(p => p.Comments)
			.HasForeignKey(c => c.ProjectId)
			.OnDelete(DeleteBehavior.NoAction);

		// User - Comment
		builder.HasOne(c => c.User)
			.WithMany(u => u.Comments)
			.HasForeignKey(c => c.UserId)
			.OnDelete(DeleteBehavior.NoAction);

		// Activity - Comment
		builder.HasOne(c => c.Activity)
			.WithMany(a => a.Comments)
			.HasForeignKey(c => c.ActivityId)
			.OnDelete(DeleteBehavior.NoAction);
	}
}
