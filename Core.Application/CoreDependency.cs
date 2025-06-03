using Application.Behaviors;
using Core.Application.Behaviors;
using Core.Application.Mapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Core.Application
{
    public static class CoreDependency
    {
        public static IServiceCollection AddCoreDependencies(this IServiceCollection services)
        {

            // Get Validators
            services.AddValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            //
            services.AddAutoMapper(typeof(ApplicationProfile));


            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingPipelineBehavior<,>));
            });

            return services;
        }
    }
}
