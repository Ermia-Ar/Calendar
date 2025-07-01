using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistance.Configurations;

public class IdentityRoleConfiguration : IEntityTypeConfiguration<IdentityRole>
{
	public void Configure(EntityTypeBuilder<IdentityRole> builder)
	{
		builder.HasKey(x => x.Id);

		var roles = new List<IdentityRole>
			{
				new IdentityRole
				{
					Id = "User",
					ConcurrencyStamp = "User",
					Name = "User",
					NormalizedName = "User".ToUpper()
				},
			};

		builder.ToTable("Roles").HasData(roles);
	}
}