using Calendar.Api.Common;
using Core.Application;
using Infrastructure.Data;
using Infrastructure.Dependency;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Share;

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

builder.Services
    .AddHttpContextAccessor();

builder.Services.AddDbContext<ApplicationContext>(option =>
    option.UseSqlServer(builder.Configuration.GetConnectionString("Connection")));

// add dependency 
builder.Services.AddCoreDependencies()
    .AddServiceDescriptors();

builder.Services
    .RegisterSharedServices();

builder.Services
    .AddApiServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseShared();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers(); 
app.Run();
