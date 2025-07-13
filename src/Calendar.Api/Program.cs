using Calendar.Api.Common;
using Core.Application;
using Infrastructure;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Serilog;
using SharedKernel;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddSwaggerGen(op =>
{
	op.MapType<TimeSpan>(() => new OpenApiSchema
	{
		Type = "String",
		Example = new OpenApiString("00:00:00")
	});

});

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

app.UseCors("AllowUI");

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();


