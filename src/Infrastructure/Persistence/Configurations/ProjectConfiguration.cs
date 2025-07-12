using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
	public void Configure(EntityTypeBuilder<Project> builder)
	{
		builder.HasKey(e => e.Id);


		builder.Property(x => x.Id)
			.UseIdentityColumn(1000, 1);

		builder.ToTable("Projects");
	}
}
