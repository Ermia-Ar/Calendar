using Core.Application.Interfaces;
using Infrastructure.Base.CurrentUserServices;
using Infrastructure.Services;
using Infrastructure.UnitOfWork;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Dependency
{
    public static class ServicesDependency
    {
        public static IServiceCollection AddServiceDescriptors(this IServiceCollection services)
        {
            services.AddScoped<IAuthServices, AuthServices>();
            services.AddScoped<ITokenServices, TokenServices>();
            services.AddScoped<IRequestServices, RequestServices>();
            services.AddScoped<IActivityServices, ActivityServices>();
            services.AddScoped<ICurrentUserServices, CurrentUserService>();
            services.AddScoped<IActivityGuestsServices, ActivityGuestsServices>();
            services.AddScoped<IUnitOfWork, UnitOfWorks>();

            return services;
        }

    }
}
