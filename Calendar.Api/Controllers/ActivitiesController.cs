using Calendar.Api.Hubs;
using Core.Application.ApplicationServices.Activities.Commands.ExitingActivity;
using Core.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Calendar.Api.Controllers;

[Route("api/[controller]/")]
[ApiController]
[Authorize]
public class ActivitiesController(ISender sender
    , IHubContext<CommonHub> hubContext,
ICurrentUserServices userServices) : ControllerBase
{
    private readonly ISender _sender = sender;
    private readonly IHubContext<CommonHub> _hubContext = hubContext;
    private readonly ICurrentUserServices _userServices = userServices;

    [HttpPost]
    public async Task<SuccessResponse> Post([FromBody] AddActivityCommandRequest request
        , CancellationToken token = default)
    {
        await _sender.Send(request, token);

        await _hubContext.Clients
            .Users(request.MemberIds).SendAsync("RequestReceive", token);

        return Result.Ok();
    }


    [HttpPost("SubActivity")]
    public async Task<SuccessResponse> PostSubActivity([FromBody] AddSubActivityCommandRequest request
         , CancellationToken token = default)
    {
        await _sender.Send(request, token);

        await _hubContext.Clients
            .Users(request.MemberIds).SendAsync("PostSubActivity", request, token);

        return Result.Ok();
    }


    [HttpPost("SubmitRequest")]
    public async Task<SuccessResponse> SendRequest([FromBody] SubmitActivityRequestCommandRequest request
        , CancellationToken token = default)
    {
        await _sender.Send(request, token);

        await _hubContext.Clients
            .Users(request.MemberIds).SendAsync("RequestReceive", token);

        return Result.Ok();
    }


    [HttpGet]
    public async Task<SuccessResponse<List<GetAllActivitiesQueryResponse>>> GetAll([FromQuery] GetAllActivitiesDto model
        , CancellationToken token = default)
    {
        var request = GetAllActivitiesQueryRequest.Create(model);
        var result = await _sender.Send(request, token);

        return Result.Ok(result);
    }

    [HttpGet("Member/{id:guid}")]
    public async Task<SuccessResponse<List<GetMemberOfActivityQueryResponse>>> GetMember(Guid id
        , CancellationToken token = default)
    {
        var request = new GetMemberOfActivityQueryRequest(id.ToString());
        var result = await _sender.Send(request, token);

        return Result.Ok(result);
    }


    [HttpGet("{id:guid}")]
    public async Task<SuccessResponse<GetActivityByIdQueryResponse>> GetById(Guid id
        , CancellationToken token = default)
    {
        var request = new GetActivityByIdQueryRequest(id.ToString());
        var result = await _sender.Send(request, token);

        return Result.Ok(result);
    }


    [HttpPut]
    public async Task<SuccessResponse> Put(UpdateActivityCommandRequest request
     , CancellationToken token = default)
    {
        await _sender.Send(request, token);

        return Result.Ok();
    }


    [HttpPatch("Complete/{id:guid:required}")]
    public async Task<SuccessResponse> Complete(Guid id
        , CancellationToken token = default)
    {
        var request = new CompleteActivityCommandRequest(id.ToString());
        await _sender.Send(request, token);

        await _hubContext.Clients
            .Group(id.ToString()).SendAsync("CompleteActivity", id.ToString(), token);

        return Result.Ok();
    }


    [HttpDelete("{id:guid:required}")]
    public async Task<SuccessResponse> Remove([FromRoute] Guid id
        , CancellationToken token = default)
    {
        var request = new DeleteActivityCommandRequest(id.ToString());
        await _sender.Send(request, token);

        await _hubContext.Clients
            .Group(id.ToString()).SendAsync("RemoveActivity", id.ToString(), token);

        return Result.Ok();
    }


    [HttpDelete("Exiting/{id:guid:required}")]
    public async Task<SuccessResponse> Exiting(Guid id
        , CancellationToken token)
    {
        var request = new ExitingActivityCommandRequest(id.ToString());
        await _sender.Send(request, token);

        var userId = _userServices.GetUserId();
        await _hubContext.Clients
             .Group(id.ToString()).SendAsync("ExitMemberActivity", id.ToString(), userId, token);

        return Result.Ok();
    }


    [HttpDelete("RemoveOf/{id:guid:required}/Member/{memberId:guid:required}")]
    public async Task<SuccessResponse> RemoveMember(Guid id, Guid memberId
        , CancellationToken token = default)
    {
        var request = new RemoveMemberOfActivityCommandRequest(id.ToString()
            , memberId.ToString());
        await _sender.Send(request, token);

        await _hubContext.Clients
           .Group(id.ToString()).SendAsync("RemoveMemberActivity", id.ToString(), memberId.ToString(), token);

        return Result.Ok();
    }
}
