using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Exceptions.Middleware;

namespace Share;

public static class ConfigServices
{
	public static IServiceCollection RegisterSharedServices(this IServiceCollection services)
	{
		services.AddScoped<MamrpExceptionHandlingMiddleware>();
		return services;
	}
	public static IApplicationBuilder UseShared(this IApplicationBuilder app)
	{
		app.UseMiddleware<MamrpExceptionHandlingMiddleware>();
		return app;
	}
}