using Core.Domain.Entities.ActivityMembers;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class ActivityMemberConfiguration : IEntityTypeConfiguration<ActivityMember>
{
    public void Configure(EntityTypeBuilder<ActivityMember> builder)
    {
        builder.ToTable("ActivityMembers");


        builder.Property(x => x.Id)
            .UseIdentityColumn(1000, 1);

		builder.HasOne(a => a.Activity)
            .WithMany(x => x.Members)
            .HasForeignKey(a => a.ActivityId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.Notification)
            .WithOne(x => x.ActivityMember)
            .HasForeignKey<ActivityMember>(a => a.NotificationId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}