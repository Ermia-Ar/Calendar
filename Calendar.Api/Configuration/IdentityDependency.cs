using Core.Domain.Entity;
using DotNetEnv;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Infrastructure.Dependency
{
    public static class IdentityDependency
    {
        public static IServiceCollection AddIdentityDependency(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentity<User, IdentityRole>()
                .AddRoles<IdentityRole>()
                .AddTokenProvider<DataProtectorTokenProvider<User>>("Auth")
                .AddEntityFrameworkStores<ApplicationContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(option =>
            {
                //user 
                option.User.RequireUniqueEmail = true;
                // password
                option.Password.RequireUppercase = false;
                option.Password.RequireLowercase = false;
                option.Password.RequiredLength = 8;
                // lock out
                option.Lockout.AllowedForNewUsers = false;
                option.Lockout.MaxFailedAccessAttempts = 3;
            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(op =>
            {
                op.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"])),
                };
            });

            return services;
        }

        //private static IServiceCollection AddAuthenticationConfig(this IServiceCollection services, IConfiguration configuration)
        //{
        //    var declarationServices = ConstantRetrieverHelper.GetConstants(typeof(SlaughterhouseServiceDeclaration));
        //    foreach (var service in declarationServices)
        //    {
        //        services.AddAuthorizationBuilder().AddPolicy(service.Value!,
        //            policy => policy.RequireRole(service.Value!));
        //    }
        //    // Needed for jwt auth.
        //    services.AddAuthentication(options =>
        //    {
        //        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        //        options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme; options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        //    })        .AddJwtBearer(options =>
        //    {
        //        options.RequireHttpsMetadata = false;
        //        options.SaveToken = true; options.TokenValidationParameters = new TokenValidationParameters
        //        {
        //            ValidIssuer = configuration["BearerTokenIssuer"], // site that makes the token
        //            ValidateIssuer = true,
        //            ValidAudience = configuration["BearerTokenAudience"], // site that consumes the token
        //            ValidateAudience = true,
        //            IssuerSigningKey =
        //                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["BearerTokenKey"]!)),
        //            ValidateIssuerSigningKey = true, // verify signature to avoid tampering
        //            ValidateLifetime = true, // validate the expiration                ClockSkew = TimeSpan.Zero // tolerance for the expiration date
        //        }; options.Events = new JwtBearerEvents
        //        {
        //            OnAuthenticationFailed = context =>
        //            {
        //                var logger = context.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>()
        //                    .CreateLogger(nameof(JwtBearerEvents)); logger.LogError("Authentication failed. Exception:{}", context.Exception);
        //                return Task.CompletedTask;
        //            },
        //            OnTokenValidated = context => { return Task.CompletedTask; },
        //            OnMessageReceived = context => Task.CompletedTask,
        //            OnChallenge = context => {
        //                var logger = context.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>().CreateLogger(nameof(JwtBearerEvents));
        //                logger.LogError("OnChallenge error Exception:{}, Description:{}", context.Error, context.ErrorDescription);
        //                return Task.CompletedTask;
        //            }
        //        };
        //    });
        //    return services;
        //}

        //public static IServiceCollection AddApiServices(this IServiceCollection services, ConfigurationManager configuration)
        //{
        //    var envFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Envs", ".Env");
        //    Env.Load(envFilePath); configuration.AddEnvironmentVariables();

        //    if (configuration["ENV"] != "production")
        //    {
        //        services.AddSwaggerConfig();
        //    }
        //    services.AddControllers(); services.AddEndpointsApiExplorer();
        //    services.AddCorsPolicy();
        //    services.AddAuthenticationConfig(configuration);
        //    return services;
        //}
    }
}
