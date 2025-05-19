using Core.Domain;
using Infrastructure.Base.CurrentUserServices;
using Infrastructure.Services;
using Core.Domain.Interfaces;
using Infrastructure.UnitOfWork;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure.Repositories;
using Core.Domain.Interfaces.Repositories;

namespace Infrastructure.Dependency
{
    public static class ServicesDependency
    {
        public static IServiceCollection AddServiceDescriptors(this IServiceCollection services)
        {
            //services.AddScoped<IAuthServices, AuthServices>();
            services.AddScoped<ICurrentUserServices, CurrentUserService>();
            services.AddScoped<IActivityRepository, ActivityRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<IRequestRepository, RequestRepository>();
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITokenServices, TokenServices>();
            services.AddScoped<IUnitOfWork, UnitOfWorks>();
            return services;
        }

    }
}
