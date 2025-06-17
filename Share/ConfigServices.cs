using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Exceptions.Middleware;

namespace Share;

public static class ConfigServices
{
    public static IServiceCollection ServicesDI(this IServiceCollection service)
    {
        service.AddScoped<MamrpExceptionHandlingMiddleware>();

        return service; 
    }

    public static IApplicationBuilder UseShared(this IApplicationBuilder app)
    {
        app.UseMiddleware<MamrpExceptionHandlingMiddleware>();
        return app;
    }
}