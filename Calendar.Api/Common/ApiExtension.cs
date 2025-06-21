using Calendar.Api.Hubs;
using Core.Domain.Entity.Users;
using Core.Domain.Exceptions;
using DotNetEnv;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace Calendar.Api.Common;
public static class ApiExtension
{
	public static IServiceCollection AddApiServices(this IServiceCollection services, ConfigurationManager configuration)
	{
		var envFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Envs", ".Env");
		Env.Load(envFilePath);
		configuration.AddEnvironmentVariables();

		if (configuration["ENV"] != "production")
		{
			services.AddSwaggerConfig();
		}
		services.AddSignalRConfig();
		services.AddControllers();
		services.AddEndpointsApiExplorer();
		services.AddAuthenticationConfig(configuration);
		return services;

	}

	private static IServiceCollection AddSignalRConfig(this IServiceCollection services)
	{
		services.AddScoped<CommonHub>();
		services.AddSignalR();

		return services;
	}

	private static IServiceCollection AddSwaggerConfig(this IServiceCollection services)
	{
#if DEBUG
		services.AddSwaggerGen(options =>
		{
			options.SwaggerDoc("v1", new OpenApiInfo { Title = "Calendar", Version = "v1" });
			options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
			{
				In = ParameterLocation.Header,
				Description = "Please enter token",
				Name = "Authorization",
				Type = SecuritySchemeType.Http,
				BearerFormat = "JWT",
				Scheme = "bearer"
			});
			options.AddSecurityRequirement(new OpenApiSecurityRequirement
			{
				{
					new OpenApiSecurityScheme
					{
						Reference = new OpenApiReference
						{
							Type = ReferenceType.SecurityScheme,
							Id = "Bearer"
						}
					},
					Array.Empty<string>()
				}
			});
		});
#endif
		return services;
	}

	private static IServiceCollection AddAuthenticationConfig(this IServiceCollection services, IConfiguration configuration)
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
				ValidIssuer = configuration["Jwt:Issuer"], // site that makes the token
				ValidateIssuer = true,
				ValidAudience = configuration["Jwt:Audience"], // site that consumes the token
				ValidateAudience = true,
				IssuerSigningKey =
					new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!)),
				ValidateIssuerSigningKey = true, // verify signature to avoid tampering
				ValidateLifetime = true, // validate the expiration                ClockSkew = TimeSpan.Zero // tolerance for the expiration date
			};
			options.Events = new JwtBearerEvents
			{
				OnAuthenticationFailed = context =>
				{
					var logger = context.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>()
						.CreateLogger(nameof(JwtBearerEvents)); logger.LogError("Authentication failed. Exception:{}", context.Exception);
					return Task.CompletedTask;
				},
				OnTokenValidated = context => { return Task.CompletedTask; },
				OnChallenge = context =>
				{
					var logger = context.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>().CreateLogger(nameof(JwtBearerEvents));
					logger.LogError("OnChallenge error Exception:{}, Description:{}", context.Error, context.ErrorDescription);
					return Task.CompletedTask;
				},
				OnMessageReceived = context =>
				{
					var accessToken = context.Request.Query["access_token"];
					var path = context.HttpContext.Request.Path;

					if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/NotificationHub"))
					{
						context.Token = accessToken;
					}

					return Task.CompletedTask;
				}
			};
		});
		return services;
	}
}