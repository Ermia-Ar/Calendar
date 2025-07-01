using Core.Domain.Entities.Activities;
using Core.Domain.Entities.Comments;
using Core.Domain.Entities.Notifications;
using Core.Domain.Entities.Projects;
using Core.Domain.Entities.Requests;
using Core.Domain.Entities.Users;
using Infrastructure.Persistence.Interceptors;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;


namespace Infrastructure.Persistance.Data;

public sealed class ApplicationContext(
	DbContextOptions options,
	FillBaseEntityValuesOnUpdatingInterceptor onUpdatingInterceptor,
	FillBaseEntityValuesOnCreatingInterceptor onCreatingInterceptor,
	SoftDeleteInterceptor softDeleteInterceptor)
	: IdentityDbContext<User>(options)
{

	private readonly FillBaseEntityValuesOnCreatingInterceptor _onCreatingInterceptor = onCreatingInterceptor;
	private readonly FillBaseEntityValuesOnUpdatingInterceptor _onUpdatingInterceptor = onUpdatingInterceptor;
	private readonly SoftDeleteInterceptor _softDeleteInterceptor = softDeleteInterceptor;


	public DbSet<Activity> Activities { get; set; }
	public DbSet<Request> Requests { get; set; }
	public DbSet<Project> Projects { get; set; }
	public DbSet<Comment> Comments { get; set; }
	public DbSet<Notification> Notifications { get; set; }

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		base.OnConfiguring(optionsBuilder);
		//optionsBuilder.AddInterceptors(_softDeleteInterceptor, _onCreatingInterceptor, _onUpdatingInterceptor);

	}
	protected override void OnModelCreating(ModelBuilder builder)
	{
		base.OnModelCreating(builder);

		builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

	}
}