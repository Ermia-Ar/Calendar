using Core.Application.Services;
using Core.Domain.Interfaces;
using Core.Domain.Interfaces.Repositories;
using Infrastructure.Base.CurrentUserServices;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Infrastructure.UnitOfWork;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Loggers;
using SharedKernel.Loggers.Abstraction;

namespace Infrastructure.Dependency;

public static class ServicesDependency
{
    public static IServiceCollection AddServiceDescriptors(this IServiceCollection services)
    {
        //services.AddScoped<IAuthServices, AuthServices>();
        services.AddScoped<ICurrentUserServices, CurrentUserService>();
        services.AddScoped<IActivitiesRepository, ActivityRepository>();
        services.AddScoped<ICommentsRepository, CommentRepository>();
        services.AddScoped<IRequestsRepository, RequestRepository>();
        services.AddScoped<IProjectsRepository, ProjectRepository>();
        services.AddScoped<ILoggerService, SerilogLoggerService>();
        services.AddScoped<IUsersRepository, UserRepository>();
        services.AddScoped<ITokenServices, TokenServices>();
        services.AddScoped<IUnitOfWork, UnitOfWorks>();

        return services;
    }

}
