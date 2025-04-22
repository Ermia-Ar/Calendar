using Core.Application.DTOs.ActivityDTOs;
using Core.Application.DTOs.UserDTOs;
using Core.Application.DTOs.UserRequestDTOs;
using Infrastructure.Entities;
using Infrastructure.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Reflection.Emit;

namespace Infrastructure.Data
{
    public class ApplicationContext : IdentityDbContext<User>
    {
        public DbSet<Activity> Activities { get; set; }
        public DbSet<UserRequest> UserRequests { get; set; }    
        public DbSet<ActivityGuest> ActivityGuests { get; set; }
 
        public ApplicationContext(DbContextOptions options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            builder.Entity<UserRequestDto>(entity =>
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


            builder.Entity<Activity>()
            .HasOne(a => a.User)
            .WithMany(u => u.Activities)
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ActivityGuest>()
                .HasKey(sc => new { sc.ActivityId, sc.UserId});

            builder.Entity<ActivityGuest>()
                .HasOne(ag => ag.User)
                .WithMany(x => x.ActivityGuests)
                .HasForeignKey(ag => ag.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ActivityGuest>()
                .HasOne(ag => ag.Activity)
                .WithMany(x => x.ActivityGuests)
                .HasForeignKey(ag => ag.ActivityId)
                .OnDelete(DeleteBehavior.Restrict);



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

            builder.Entity<IdentityRole>().ToTable("Roles").HasData(roles);
            builder.Entity<User>().ToTable("Users");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
        }
    }
}
