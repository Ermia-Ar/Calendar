using Calendar.Api.Common;
using Core.Application;
using Infrastructure;
using Infrastructure.Persistance.Data;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Share;
using SharedKernel;

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
builder.Services.AddApplicatinoConfig()
    .AddInfraConfig()
    .AddShared(builder.Configuration);

builder.Services.RegisterSharedServices()
        .AddApiServices(builder.Configuration);

builder.Services.AddHttpContextAccessor();

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

app.MapHub<CommonHub>("/CommonHub");

app.Run();
