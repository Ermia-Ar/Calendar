using Microsoft.AspNetCore.Http;
using Share.Abstract;
using SharedKernel.ResponseResult;
using System.Text.Json;

namespace Share.Middlewere;

public sealed class BadRequestExceptionMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (BadRequestException ex)
        {
            context.Response.StatusCode = 400;
            context.Response.Headers.Append("content-type", "application/json");
            var json = JsonSerializer.Serialize(Result.Fail("" , ex.Message));
            await context.Response.WriteAsync(json);
        }
    }
}
