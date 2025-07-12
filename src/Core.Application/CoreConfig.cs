using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Behavior;
using System.Reflection;

namespace Core.Application;

public static class CoreConfig
{
	public static IServiceCollection AddApplicationServices(this IServiceCollection services)
	{
		// Get Validators
		services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

		//
		services.AddMediatR(cfg =>
		{
			cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
			cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
			cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
		});

		return services;
	}
}
