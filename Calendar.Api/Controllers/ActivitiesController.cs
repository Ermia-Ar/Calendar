
using Calendar.Api.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Calendar.Api.Controllers;

[Route("api/[controller]/")]
[ApiController]
[Authorize]
public class ActivitiesController(ISender sender
    , IHubContext<CalendarSyncHub> hubCalendar
    , IHubContext<PresenceHub> hubPresence) : ControllerBase
{
    private readonly ISender _sender = sender;
    private readonly IHubContext<CalendarSyncHub> _hubCalendar = hubCalendar;
    private readonly IHubContext<PresenceHub> _hubPresence = hubPresence;

    [HttpPost]
    //[Authorize(CalendarClaims.CreateActivity)]
    public async Task<SuccessResponse> Post([FromBody] AddActivityCommandRequest request
        , CancellationToken token = default)
    {
        await _sender.Send(request, token);

        return Result.Ok();
    }

    [HttpPost("SubActivity")]
    //[Authorize(CalendarClaims.CreateSubActivity)]
    public async Task<SuccessResponse> PostSubActivity([FromBody] AddSubActivityCommandRequest request
         , CancellationToken token = default)
    {
        await _sender.Send(request, token);
        await _hubCalendar.Clients
            .Group(request.ActivityId).SendAsync("AddSubActivity", request, token);

        return Result.Ok();
    }

    [HttpPost("SubmitRequest")]
    //[Authorize(CalendarClaims.SendActivityRequest)]
    public async Task<SuccessResponse> SendRequest([FromBody] SubmitActivityRequestCommandRequest request
        , CancellationToken token = default)
    {
        await _sender.Send(request, token);

        return Result.Ok();
    }

    [HttpGet]
    //[Authorize(CalendarClaims.GetAllUserActivity)]
    public async Task<SuccessResponse<List<GetAllActivitiesQueryResponse>>> GetAll([FromQuery] GetAllActivitiesDto model
        , CancellationToken token = default)
    {
        var request = GetAllActivitiesQueryRequest.Create(model);
        var result = await _sender.Send(request, token);

        return Result.Ok(result);
    }

    [HttpGet("GetMember/{id:guid}")]
    //[Authorize(CalendarClaims.GetMemberOfActivity)]
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
    //[Authorize(CalendarClaims.UpdateActivity)]
    public async Task<SuccessResponse> Put(UpdateActivityCommandRequest request
     , CancellationToken token = default)
    {
        await _sender.Send(request, token);

        return Result.Ok();
    }

    [HttpPatch("Complete/{id:guid:required}")]
    //[Authorize(CalendarClaims.CompleteActivity)]
    public async Task<SuccessResponse> Complete(Guid id
        , CancellationToken token = default)
    {
        var request = new CompleteActivityCommandRequest(id.ToString());
        await _sender.Send(request, token);

        return Result.Ok();
    }

    [HttpDelete("{id:guid:required}")]
    //[Authorize(CalendarClaims.DeleteActivity)]
    public async Task<SuccessResponse> Remove([FromRoute] Guid id
        , CancellationToken token = default)
    {
        var request = new DeleteActivityCommandRequest(id.ToString());
        await _sender.Send(request, token);

        return Result.Ok();
    }

    [HttpDelete("RemoveOf/{id:guid:required}/Member/{memberId:guid:required}")]
    //[Authorize(CalendarClaims.RemoveMemberOfActivity)]
    public async Task<SuccessResponse> RemoveMember(Guid id, Guid memberId
        , CancellationToken token = default)
    {
        var request = new RemoveMemberOfActivityCommandRequest(id.ToString()
            , memberId.ToString());
        await _sender.Send(request, token);

        return Result.Ok();
    }
}
