using Core.Domain.Entities.ActivityMembers;
using Core.Domain.Entities.ProjectMembers;
using Infrastructure.Persistence.Interceptors;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Reflection;
using Infrastructure.Persistence.Configurations;


namespace Infrastructure.Persistence.Context;

public sealed class ApplicationContext(
	DbContextOptions options,
	FillBaseEntityValuesOnUpdatingInterceptor onUpdatingInterceptor,
	FillBaseEntityValuesOnCreatingInterceptor onCreatingInterceptor,
	SoftDeleteInterceptor softDeleteInterceptor)
	: DbContext(options)
{

	private readonly FillBaseEntityValuesOnCreatingInterceptor _onCreatingInterceptor = onCreatingInterceptor;
	private readonly FillBaseEntityValuesOnUpdatingInterceptor _onUpdatingInterceptor = onUpdatingInterceptor;
	private readonly SoftDeleteInterceptor _softDeleteInterceptor = softDeleteInterceptor;


	public DbSet<Activity> Activities { get; set; }
	public DbSet<Request> Requests { get; set; }
	public DbSet<Project> Projects { get; set; }
	public DbSet<Comment> Comments { get; set; }
	public DbSet<Notification> Notifications { get; set; }
	public DbSet<ActivityMember> ActivityMembers { get; set; }
	public DbSet<ProjectMember> ProjectMembers { get; set; }

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		base.OnConfiguring(optionsBuilder);
		optionsBuilder.AddInterceptors(_softDeleteInterceptor, _onCreatingInterceptor, _onUpdatingInterceptor);

	}
	protected override void OnModelCreating(ModelBuilder builder)
	{
		base.OnModelCreating(builder);

		builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
		builder.ConfigSoftDeleteFilter();

		builder.HasDefaultSchema("Calendar");

	}
}