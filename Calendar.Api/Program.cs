using Calendar.Api.Common;
using Calendar.Api.Hubs;
using Core.Application;
using Infrastructure.Data;
using Infrastructure.Dependency;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Share;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, services, configuration) =>
{
    configuration
        .ReadFrom.Configuration(context.Configuration)
        .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day);
});
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle


builder.Services.AddDbContext<ApplicationContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("Connection"));
    option.EnableSensitiveDataLogging();
});

// add dependency 
builder.Services.AddCoreDependencies()
    .AddServiceDescriptors();

builder.Services.ServicesDI()
    .AddApiServices(builder.Configuration);

builder.Services.AddHttpContextAccessor();



builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowUI", policy =>
	{
		policy.WithOrigins("https://localhost:7190") 
			  .AllowAnyHeader()
			  .AllowAnyMethod()
			  .AllowCredentials(); 
	});
});

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly()); 

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseShared();

app.UseCors("AllowUI");

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapHub<CommonHub>("/CommonHub")
	.RequireCors("AllowUI")
	.RequireAuthorization();

app.Run();
