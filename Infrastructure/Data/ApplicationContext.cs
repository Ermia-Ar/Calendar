using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Core.Domain.Entity.Activities;
using Core.Domain.Entity.Comments;
using Core.Domain.Entity.Projects;
using Core.Domain.Entity.UserRequests;
using Core.Domain.Entity.Users;


namespace Infrastructure.Data
{
    public class ApplicationContext : IdentityDbContext<User>
    {
        public DbSet<Activity> Activities { get; set; }
        public DbSet<UserRequest> UserRequests { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Comment> Comments { get; set; }

        public ApplicationContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            // User - Project
            builder.Entity<Project>()
                .HasOne(p => p.User)
                .WithMany(u => u.Projects)
                .HasForeignKey(p => p.OwnerId)
                .OnDelete(DeleteBehavior.NoAction);

            // User - Comment
            builder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            // User - Activity
            builder.Entity<Activity>()
                .HasOne(a => a.User)
                .WithMany(u => u.Activities)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            // User - UserRequest (Sender)
            builder.Entity<UserRequest>()
                .HasOne(r => r.Sender)
                .WithMany(u => u.SendRequests)
                .HasForeignKey(r => r.SenderId)
                .OnDelete(DeleteBehavior.NoAction);

            // User - UserRequest (Receiver)
            builder.Entity<UserRequest>()
                .HasOne(r => r.Receiver)
                .WithMany(u => u.ReceiveRequests)
                .HasForeignKey(r => r.ReceiverId)
                .OnDelete(DeleteBehavior.NoAction);

            // Project - Activity
            builder.Entity<Activity>()
                .HasOne(a => a.Project)
                .WithMany(p => p.Activities)
                .HasForeignKey(a => a.ProjectId)
                .OnDelete(DeleteBehavior.NoAction);

            // Project - Comment
            builder.Entity<Comment>()
                .HasOne(c => c.Project)
                .WithMany(p => p.Comments)
                .HasForeignKey(c => c.ProjectId)
                .OnDelete(DeleteBehavior.NoAction);

            // Project - UserRequest
            builder.Entity<UserRequest>()
                .HasOne(r => r.Project)
                .WithMany(p => p.UserRequests)
                .HasForeignKey(r => r.ProjectId)
                .OnDelete(DeleteBehavior.NoAction);

            // Activity - Comment
            builder.Entity<Comment>()
                .HasOne(c => c.Activity)
                .WithMany(a => a.Comments)
                .HasForeignKey(c => c.ActivityId)
                .OnDelete(DeleteBehavior.NoAction);

            // Activity - UserRequest
            builder.Entity<UserRequest>()
                .HasOne(r => r.Activity)
                .WithMany(a => a.UserRequests)
                .HasForeignKey(r => r.ActivityId)
                .OnDelete(DeleteBehavior.NoAction);

            // Activity - SubActivities (Self-referencing)
            builder.Entity<Activity>()
                .HasOne(a => a.Parent)
                .WithMany(a => a.SubActivities)
                .HasForeignKey(a => a.ParentId)
                .OnDelete(DeleteBehavior.NoAction);

            var project = new Project
            {
                Id = "8c56ac14-ae28-4425-9a19-690d27d3a16d",
                OwnerId = "05e404b3-e235-4c11-bff4-3754b22c0245",
                Title = "Public Project",
                StartDate = DateTime.Now,
                EndDate = DateTime.MaxValue,
                CreatedDate = DateTime.Now,
                Description = "this is static project",
                UpdateDate = DateTime.Now,
            };

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

            builder.Entity<Project>().ToTable("Projects").HasData(project);
            builder.Entity<Comment>().ToTable("Comments");
            builder.Entity<IdentityRole>().ToTable("Roles").HasData(roles);
            builder.Entity<User>().ToTable("Users");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
        }
    }
}
