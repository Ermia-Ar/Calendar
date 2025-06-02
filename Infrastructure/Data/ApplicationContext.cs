using Core.Domain.Entity;
using UserModel = Infrastructure.Models.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class ApplicationContext : IdentityDbContext<UserModel>
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


            //builder.Entity<ActivityRequestDto>(entity =>
            //{
            //    entity.HasNoKey();
            //});

            //builder.Entity<UserResponse>(entity =>
            //{
            //    entity.HasNoKey();
            //});

            //builder.Entity<ActivityResponse>(entity =>
            //{
            //    entity.HasNoKey();
            //});

            //builder.Entity<ProjectRequestsDTO>(entity =>
            //{
            //    entity.HasNoKey();
            //});

            builder.Entity<User>()
                .HasMany(u => u.SendRequests)
                .WithOne(ur => ur.Sender)
                .HasForeignKey(ur => ur.SenderId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<User>()
                .HasMany(u => u.ReceiveRequests)
                .WithOne(ur => ur.Receiver)
                .HasForeignKey(ur => ur.ReceiverId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Activity>()
                .HasOne(x => x.Parent)
                .WithMany(x => x.SubActivities)
                .HasForeignKey(x => x.ParentId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Comment>()
                .HasOne(x => x.User)
                .WithMany(x => x.Comments)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Comment>()
                .HasOne(x => x.Activity)
                .WithMany(x => x.Comments)
                .HasForeignKey(x => x.ActivityId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Activity>()
                .HasOne(a => a.Project)
                .WithMany(p => p.Activities)
                .HasForeignKey(a => a.ProjectId);

            builder.Entity<Comment>()
                .HasOne(x => x.Project)
                .WithMany(x => x.Comments)
                .HasForeignKey(x => x.ProjectId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Activity>()
                .HasOne(a => a.User)
                .WithMany(u => u.Activities)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Project>()
                .HasOne(p => p.User)
                .WithMany()
                .HasForeignKey(p => p.OwnerId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Activity>()
            .HasOne(a => a.User)
            .WithMany(u => u.Activities)
            .HasForeignKey(a => a.UserId)
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
            builder.Entity<UserModel>().ToTable("Users");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
        }
    }
}
