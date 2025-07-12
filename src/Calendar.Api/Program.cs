using Calendar.Api.Common;
using Core.Application;
using Infrastructure;
using Serilog;
using SharedKernel;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// add dependency 
builder.Services
    .AddApplicationServices()
    .AddInfraServices(builder.Configuration)
    .AddShared(builder.Configuration)
    .AddApiServices(builder.Configuration);


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerConfig();
}

app.UseExceptionHandling();

app.UseSerilogRequestLogging();

app.UseCors("AllowUI");

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
