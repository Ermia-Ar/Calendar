using Core.Application.Services;
using Core.Domain.Interfaces;
using Core.Domain.Interfaces.Repositories;
using Infrastructure.Base.CurrentUserServices;
using Infrastructure.Mapper;
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
        services.AddScoped<IActivityRepository, ActivityRepository>();
        services.AddScoped<ICommentRepository, CommentRepository>();
        services.AddScoped<IRequestRepository, RequestRepository>();
        services.AddScoped<IProjectRepository, ProjectRepository>();
        services.AddScoped<ILoggerService, SerilogLoggerService>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ITokenServices, TokenServices>();
        services.AddScoped<IUnitOfWork, UnitOfWorks>();
        services.AddAutoMapper(typeof(InfraProfile));

        return services;
    }

}
