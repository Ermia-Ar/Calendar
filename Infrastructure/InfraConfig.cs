using Infrastructure.ExternalServices;
using Infrastructure.Persistance;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class InfraConfig
{
	public static IServiceCollection AddInfraConfig(this IServiceCollection services)
	{
		services.AddExternalConfig()
			.AddPersistanceConfig();

		return services;
	}

}
