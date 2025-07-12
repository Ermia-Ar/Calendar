using Core.Domain.Entities.ProjectMembers;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class ProjectMemberConfiguration : IEntityTypeConfiguration<ProjectMember>
{
    public void Configure(EntityTypeBuilder<ProjectMember> builder)
    {
        builder.ToTable("ProjectMembers");


        builder.Property(x => x.Id)
            .UseIdentityColumn(1000, 1);

		builder.HasOne(a => a.Project)
            .WithMany(x => x.Members)
            .HasForeignKey(a => a.ProjectId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}