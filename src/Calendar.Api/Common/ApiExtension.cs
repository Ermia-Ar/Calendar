using Core.Domain.Exceptions;
using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SharedKernel;
using System.Text;

namespace Calendar.Api.Common;
public static class ApiExtension
{
	public static IServiceCollection AddApiServices(this IServiceCollection services, ConfigurationManager configuration)
	{
		var envFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Envs", ".Env");
		Env.Load(envFilePath);
		configuration.AddEnvironmentVariables();

		services.AddHttpContextAccessor();
		services.AddCorsConfig();
		services.AddControllers();
		services.AddEndpointsApiExplorer();
		services.AddSwaggerConfig();
		//services.AddAuthenticationConfig(configuration);
		return services;

	}

	private static IServiceCollection AddCorsConfig(this IServiceCollection services)
	{
		services.AddCors(options =>
		{
			options.AddPolicy("AllowUI", policy =>
			{
				policy.WithOrigins("https://localhost:7190")
					  .AllowAnyHeader()
					  .AllowAnyMethod()
					  .AllowCredentials();
			});
		});

		return services;
	}


	private static IServiceCollection AddAuthenticationConfig(this IServiceCollection services, IConfiguration configuration)
	{
		var declarationServices = ConstantRetrieverHelper.GetConstants(typeof(CalendarClaimsServiceDeclaration));
		foreach (var service in declarationServices)
		{
			services.AddAuthorizationBuilder().AddPolicy(service.Value!,
				policy => policy.RequireRole(service.Value!));
		}
		//Needed for jwt auth.
		services.AddAuthentication(options =>
		{
			options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme; options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
		}).AddJwtBearer(options =>
		{
			options.RequireHttpsMetadata = false;
			options.SaveToken = true;
			options.TokenValidationParameters = new TokenValidationParameters
			{
				ValidIssuer = configuration["JWT:ISSUER"], // site that makes the token
				ValidateIssuer = true,
				ValidAudience = configuration["JWT:AUDIENCE"], // site that consumes the token
				ValidateAudience = true,
				IssuerSigningKey =
					new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:KEY"])),
				ValidateIssuerSigningKey = true, // verify signature to avoid tampering
				ValidateLifetime = true, // validate the expiration
				ClockSkew = TimeSpan.Zero // tolerance for the expiration date
			};
			options.Events = new JwtBearerEvents
			{
				OnAuthenticationFailed = context =>
				{
					var logger = context.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>()
						.CreateLogger(nameof(JwtBearerEvents)); 

					logger.LogError("Authentication failed. Exception:{}", context.Exception);
					return Task.CompletedTask;
				},
				OnTokenValidated = context => { return Task.CompletedTask; },
				OnChallenge = context =>
				{
					var logger = context.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>().CreateLogger(nameof(JwtBearerEvents));
					logger.LogError("OnChallenge error Exception:{}, Description:{}", context.Error, context.ErrorDescription);
					return Task.CompletedTask;
				},
			};
		});
		return services;
	}
}