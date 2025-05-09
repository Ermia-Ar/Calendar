using Core.Application.DTOs.ActivityDTOs;
using Core.Application.DTOs.UserDTOs;
using Core.Application.DTOs.UserRequestDTOs;
using Core.Domain.Entity;
using Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class ApplicationContext : IdentityDbContext<User>
    {
        public DbSet<Activity> Activities { get; set; }
        public DbSet<UserRequest> UserRequests { get; set; }    
        public DbSet<Project> Projects { get; set; }
 
        public ApplicationContext(DbContextOptions options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ActivityRequestDto>(entity =>
            {
                entity.HasNoKey();
            });

            builder.Entity<UserResponse>(entity =>
            {
                entity.HasNoKey();
            });

            builder.Entity<ActivityResponse>(entity =>
            {
                entity.HasNoKey();
            });

            builder.Entity<ProjectRequestsDTO>(entity =>
            {
                entity.HasNoKey();
            });

            builder.Entity<Activity>()
                .HasOne(a => a.Project)
                .WithMany(p => p.Activities)
                .HasForeignKey(a => a.ProjectId);

            builder.Entity<Activity>()
                .HasOne(a => a.User)
                .WithMany(u => u.Activities)
                .HasForeignKey(a => a.UserId);

            builder.Entity<Project>()
                .HasOne(p => p.User)
                .WithMany()
                .HasForeignKey(p => p.OwnerId)
                .OnDelete(DeleteBehavior.Restrict); 

            builder.Entity<Activity>()
            .HasOne(a => a.User)
            .WithMany(u => u.Activities)
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.Cascade);

            var projects = new List<Project>()
            {
                new Project
                {
                    OwnerId = "05e404b3-e235-4c11-bff4-3754b22c0245",
                    Title = "Public Project",
                    Id = "8c56ac14-ae28-4425-9a19-690d27d3a16d",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.MaxValue,
                    CreatedDate = DateTime.Now,
                    Description = "this is static project",
                    UpdateDate = DateTime.Now,
                }
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

            builder.Entity<Project>().ToTable("Projects").HasData(projects);
            builder.Entity<IdentityRole>().ToTable("Roles").HasData(roles);
            builder.Entity<User>().ToTable("Users");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
        }
    }
}
