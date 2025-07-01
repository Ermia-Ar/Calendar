using Core.Domain.Entities.Requests;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistance.Configurations;

public class RequestConfiguration : IEntityTypeConfiguration<Request>
{
	public void Configure(EntityTypeBuilder<Request> builder)
	{
		builder.ToTable("Requests");

		// User - UserRequest (Receiver)
		builder.HasOne(r => r.Receiver)
			.WithMany(u => u.ReceiveRequests)
			.HasForeignKey(r => r.ReceiverId)
			.OnDelete(DeleteBehavior.NoAction);

		// User - UserRequest (Sender)
		builder.HasOne(r => r.Sender)
			.WithMany(u => u.SendRequests)
			.HasForeignKey(r => r.SenderId)
			.OnDelete(DeleteBehavior.NoAction);

		// Project - UserRequest
		builder.HasOne(r => r.Project)
			.WithMany(p => p.UserRequests)
			.HasForeignKey(r => r.ProjectId)
			.OnDelete(DeleteBehavior.NoAction);

		// Activity - UserRequest
		builder.HasOne(r => r.Activity)
			.WithMany(a => a.UserRequests)
			.HasForeignKey(r => r.ActivityId)
			.OnDelete(DeleteBehavior.NoAction);

		// request - notification 
		builder.HasOne(x => x.Notification)
			.WithOne(x => x.Request)
			.OnDelete(DeleteBehavior.NoAction);
	}
}
