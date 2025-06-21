
using Calendar.Api.Hubs;
using Core.Application.ApplicationServices.UserRequests.Queries.GetAll;
using Core.Application.ApplicationServices.UserRequests.Queries.GetById;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Calendar.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]

public class UserRequestsController(ISender sender, IHubContext<CommonHub> hubContext) : ControllerBase
{
    private readonly ISender _sender = sender;
    private readonly IHubContext<CommonHub> _hubContext = hubContext;

	[HttpPost("Answer")]
    //[Authorize(CalendarClaims.AnswerRequest)]
    public async Task<SuccessResponse> Answer([FromQuery]AnswerRequestCommandRequest request
        , CancellationToken token = default)
    {
        await _sender.Send(request, token);

        //send to sender request and send to group for this activity/project
        //await _hubContext.Clients
        //    .User().SendAsync("AnswerRequest", token);

        return Result.Ok();
    }

    [HttpDelete("{id:guid}")]
    //[Authorize(CalendarClaims.RemoveRequest)]
    public async Task<SuccessResponse> Remove([FromRoute] Guid id,
        CancellationToken token = default)
    {
        var request = new DeleteRequestCommandRequest(id.ToString());
        await _sender.Send(request);

        //send to receiver request
        //await _hubContext.Clients
        //        .User().SendAsync("RemoveRequest", token);

        return Result.Ok();
    }

    [HttpGet("{id:guid:required}")]
    public async Task<SuccessResponse<GetRequestByIdQueryResponse>> GetById(Guid id
        , CancellationToken token)
    {
        var request = new GetRequestByIdQueryRequest(id.ToString());
        var result = await _sender.Send(request, token);

        return Result.Ok(result);
    }

    [HttpGet("All")]
    public async Task<SuccessResponse<List<GetAllRequestQueryResponse>>> GetAll([FromQuery] GetAllRequestDto model
        , CancellationToken token)
    {
        var request = GetAllRequestsQueryRequest.Create(model);
        var result = await _sender.Send(request, token);   

        return Result.Ok(result);
    }

}
