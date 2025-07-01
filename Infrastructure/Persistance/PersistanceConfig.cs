using Core.Application.Common;
using Core.Application.ExternalServices.Jwt;
using Core.Domain.Repositories;
using Core.Domain.UnitOfWork;
using Infrastructure.ExternalServices.CurrentUserServices;
using Infrastructure.ExternalServices.Jwt;
using Infrastructure.ExternalServices.SignalR;
using Infrastructure.Persistance.Repositories;
using Infrastructure.Persistance.UnitOfWork;
using Infrastructure.Persistence.Interceptors;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Loggers;
using SharedKernel.Loggers.Abstraction;

namespace Infrastructure.Persistance;

public static class PersistanceConfig
{
	public static IServiceCollection AddPersistanceConfig(this IServiceCollection services)
	{
		services.AddScoped<INotificationRepository, NotificationRepository>();
		services.AddScoped<IActivitiesRepository, ActivityRepository>();
		services.AddScoped<ICommentsRepository, CommentRepository>();
		services.AddScoped<IRequestsRepository, RequestRepository>();
		services.AddScoped<IProjectsRepository, ProjectRepository>();
		services.AddScoped<IUsersRepository, UserRepository>();
		services.AddScoped<IUnitOfWork, UnitOfWorks>();


		services.AddScoped<FillBaseEntityValuesOnCreatingInterceptor>();
		services.AddScoped<FillBaseEntityValuesOnUpdatingInterceptor>();
		services.AddScoped<SoftDeleteInterceptor>();

		return services;
	}
}
