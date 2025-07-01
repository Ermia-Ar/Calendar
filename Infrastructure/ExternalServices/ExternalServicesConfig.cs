namespace Infrastructure.ExternalServices;

public static class ExternalServicesConfig
{
	public static IServiceCollection AddExternalConfig(this IServiceCollection services)
	{
		services.AddScoped<ICurrentUserServices, CurrentUserService>();
		services.AddScoped<IGroupServices, GroupServices>();
		services.AddScoped<ILoggerService, SerilogLoggerService>();
		services.AddScoped<ITokenServices, TokenServices>();
		services.AddSingleton<IOnlineClientManager, OnlineClientManager>();

		return services;
	}
}
