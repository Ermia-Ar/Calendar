namespace Infrastructure.ExternalServices;

public static class ExternalServicesConfig
{
	public static IServiceCollection AddExternalConfig(this IServiceCollection services)
	{
		services.AddScoped<ICurrentUserServices, CurrentUserService>();
		services.AddScoped<ILoggerService, SerilogLoggerService>();

		return services;
	}
}
