using Core.Domain.Entity;
using Core.Domain.Identity;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Infrastructure.Dependency
{
    public static class IdentityDependency
    {
        public static IServiceCollection AddIdentityDependency(this IServiceCollection services)
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

            //services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            //    .AddCookie(options =>
            //    {
            //        options.ExpireTimeSpan = TimeSpan.FromDays(14);
            //        options.SlidingExpiration = true;
            //    });
            //services.AddAuthorization();



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
                    ValidIssuer = JwtSettings.Issuer,
                    ValidAudience = JwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtSettings.Key)),
                };
            });


            return services;
        }
    }
}
