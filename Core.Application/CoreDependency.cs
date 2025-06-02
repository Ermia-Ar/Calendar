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
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            //
            services.AddAutoMapper(typeof(ApplicationProfile));


            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingPipelineBehavior<,>));
            });

            return services;
        }
    }
}
