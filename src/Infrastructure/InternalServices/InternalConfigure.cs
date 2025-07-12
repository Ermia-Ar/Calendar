using Core.Application.InternalServices.Auth.Services;
using Infrastructure.InternalServices.Auth;

namespace Infrastructure.InternalServices;

public static class InternalConfigure
{
	public static IServiceCollection AddInternalConfig(this IServiceCollection services)
	{
		services.AddScoped<IUserSrevices, UserServices>();
		services.AddHttpClient();
		
		return services;
	}
}
