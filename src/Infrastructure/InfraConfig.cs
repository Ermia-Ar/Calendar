using Infrastructure.ExternalServices;
using Infrastructure.InternalServices;
using Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class InfraConfig
{
	public static IServiceCollection AddInfraServices(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddExternalConfig()
			.AddInternalConfig()
			.AddPersistanceConfig(configuration);


		return services;
	}

}
