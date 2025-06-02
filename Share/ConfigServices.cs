using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Share.Middlewere;

namespace Share;

public static class ConfigServices
{
    public static IServiceCollection RegisterSharedServices(this IServiceCollection services)
    {
        services.AddScoped<BadRequestExceptionMiddleware>();
        services.AddScoped<NotFoundExceptionMiddleware>();
        return services;
    }
    public static IApplicationBuilder UseShared(this IApplicationBuilder app)
    {
        app.UseMiddleware<BadRequestExceptionMiddleware>();
        app.UseMiddleware<NotFoundExceptionMiddleware>();
        return app;
    }
}