using Core.Domain.Entities.Projects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistance.Configurations;

public class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
	public void Configure(EntityTypeBuilder<Project> builder)
	{
		builder.ToTable("Projects").HasData(ProjectFactory.GetDefault());

		// User - Project
		builder.HasOne(p => p.User)
			.WithMany(u => u.Projects)
			.HasForeignKey(p => p.OwnerId)
			.OnDelete(DeleteBehavior.NoAction);
	}
}
