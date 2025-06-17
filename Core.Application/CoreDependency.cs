using Application.Behaviors;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Behavior;

namespace Core.Application;

public static class CoreDependency
{
    public static IServiceCollection AddCoreDependencies(this IServiceCollection services)
    {
        // Get Validators
        services.AddValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
        //services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        //
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingPipelineBehavior<,>));
        });

        return services;
    }
}
