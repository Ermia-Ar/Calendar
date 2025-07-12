using Core.Domain.UnitOfWork;
using Infrastructure.Persistence.Context;
using Infrastructure.Persistence.Interceptors;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Persistence.UnitOfWork;

namespace Infrastructure.Persistence;

public static class PersistanceConfig
{
	public static IServiceCollection AddPersistanceConfig(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddScoped<IActivityMembersRepository, ActivityMembersRepository>();
		services.AddScoped<IProjectMembersRepository, ProjectMembersRepository>();
		services.AddScoped<INotificationRepository, NotificationsRepository>();
		services.AddScoped<IActivitiesRepository, ActivitiesRepository>();
		services.AddScoped<ICommentsRepository, CommentsRepository>();
		services.AddScoped<IRequestsRepository, RequestsRepository>();
		services.AddScoped<IProjectsRepository, ProjectsRepository>();
		services.AddScoped<IUsersRepository, UsersRepository>();
		services.AddScoped<IUnitOfWork, UnitOfWorks>();


		services.AddScoped<FillBaseEntityValuesOnCreatingInterceptor>();
		services.AddScoped<FillBaseEntityValuesOnUpdatingInterceptor>();
		services.AddScoped<SoftDeleteInterceptor>();


		services.AddDbContext<ApplicationContext>(option =>
		{
			option.UseSqlServer(configuration["CONNECTIONSTRINGS:CONNECTION"]);
			option.EnableSensitiveDataLogging();
		});


		return services;
	}
}
