using Amazon.S3.Model.Internal.MarshallTransformations;
using Calendar.Api.Abstraction;
using Core.Application.ApplicationServices.Activities.Commands.Add;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.ResponseResult;

namespace Calendar.Api.Endpoint.Activity.AddActivity;

public class AddActivityEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/activity/", async (
                [FromBody] AddActivityCommandRequest request,
                IMediator mediator,
                CancellationToken cancellationToken) =>
        {
            await mediator.Send(request,cancellationToken);
            return Result.Ok();
        });   
    }
}

