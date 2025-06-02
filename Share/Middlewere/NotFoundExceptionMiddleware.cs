using Microsoft.AspNetCore.Http;
using Share.Abstract;
using SharedKernel.ResponseResult;
using System.Text.Json;

namespace Share.Middlewere;

public sealed class NotFoundExceptionMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (NotFoundException ex)
        {
            context.Response.StatusCode = 404;
            context.Response.Headers.Add("content-type", "application/json");
            var json = JsonSerializer.Serialize(Result.Fail("20001" , ex.Message));
            await context.Response.WriteAsync(json);
        }
    }
}