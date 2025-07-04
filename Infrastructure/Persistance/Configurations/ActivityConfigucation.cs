﻿using Core.Domain.Entities.Activities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistance.Configurations;

public class ActivityConfigucation : IEntityTypeConfiguration<Activity>
{
	public void Configure(EntityTypeBuilder<Activity> builder)
	{
		builder.ToTable("Activities");

		//User - Activity
		builder.HasOne(a => a.User)
			.WithMany(u => u.Activities)
			.HasForeignKey(a => a.UserId)
			.OnDelete(DeleteBehavior.NoAction);

		// Project - Activity
		builder.HasOne(a => a.Project)
			.WithMany(p => p.Activities)
			.HasForeignKey(a => a.ProjectId)
			.OnDelete(DeleteBehavior.NoAction);

		// Activity - SubActivities (Self-referencing)
		builder.HasOne(a => a.Parent)
			.WithMany(a => a.SubActivities)
			.HasForeignKey(a => a.ParentId)
			.OnDelete(DeleteBehavior.NoAction);
	}
}
