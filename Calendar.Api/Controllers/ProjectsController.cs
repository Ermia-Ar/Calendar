using Calendar.Api.Hubs;
using Core.Application.ApplicationServices.Projects.Commands.Exiting;
using Core.Domain.Entity.Users;
using Core.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Calendar.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]

public class ProjectsController(ISender sender, IHubContext<CommonHub> hubContext, ICurrentUserServices currentUserServices) : ControllerBase
{

    private ISender _sender = sender;
    private readonly IHubContext<CommonHub> _hubContext = hubContext;
    private readonly ICurrentUserServices _currentUserServices = currentUserServices;

    [HttpPost]
    public async Task<SuccessResponse> Add([FromBody] AddProjectCommandRequest request
        , CancellationToken token = default)
    {
        await _sender.Send(request, token);

        return Result.Ok();
    }


    [HttpPost("SubmitRequest")]
    public async Task<SuccessResponse> SendProjectRequest([FromBody] SubmitProjectRequestCommandRequest request
        , CancellationToken token = default)
    {
        await _sender.Send(request, token);

        await _hubContext.Clients
            .Users(request.MemberIds).SendAsync("RequestReceive", token);

        return Result.Ok();
    }


    [HttpPost("Activities")]
    public async Task<SuccessResponse> AddActivity([FromBody] AddActivityForProjectCommandRequest request
    , CancellationToken token = default)
    {
        await _sender.Send(request, token);

        await _hubContext.Clients
            .Users(request.MemberIds).SendAsync("AddActivityToProject", request, token);

        return Result.Ok();
    }


    [HttpGet("Members/{id:guid}")]
    public async Task<SuccessResponse<List<GetMemberOfProjectQueryResponse>>> GetMembers([FromRoute] Guid id
        , CancellationToken token = default)
    {
        var request = new GetMemberOfProjectQueryRequest(id.ToString());
        var result = await _sender.Send(request, token);


        return Result.Ok(result);
    }


    [HttpGet("Activities/{id:guid}")]
    public async Task<SuccessResponse<List<GetActivityOfProjectQueryResponse>>> GetActivities([FromRoute] Guid id
        , CancellationToken token = default)
    {
        var request = new GetActivitiesOfProjectQueryRequest(id.ToString());
        var result = await _sender.Send(request, token);

        return Result.Ok(result);
    }


    [HttpGet]
    public async Task<SuccessResponse<List<GetAllProjectQueryResponse>>> GetAll([FromQuery] GetAllProjectDto model
        , CancellationToken token = default)
    {
        var request = GetAllProjectsQueryRequest.Create(model);
        var result = await _sender.Send(request, token);

        return Result.Ok(result);
    }


    [HttpGet("{id:guid:required}")]
    public async Task<SuccessResponse<GetProjectByIdQueryResponse>> GetById(Guid id
        , CancellationToken token = default)
    {
        var request = new GetProjectByIdQueryRequest(id.ToString());
        var result = await _sender.Send(request, token);

        return Result.Ok(result);
    }


    [HttpDelete("Exiting/{id:guid:required}")]
    public async Task<SuccessResponse> Exiting(Guid id
      , CancellationToken token)
    {
        var request = new ExitingProjectCommandRequest(id.ToString());
        await _sender.Send(request, token);

        var userId = _currentUserServices.GetUserId();
        await _hubContext.Clients
            .Group(id.ToString()).SendAsync("ExitMemberProject", id, userId, token);

        return Result.Ok();
    }


    [HttpDelete("RemoveOf/{id:guid:required}/Member/{memberId:guid:required}")]
    public async Task<SuccessResponse> RemoveMember(Guid id, Guid memberId,
        CancellationToken token = default)
    {
        var request = new RemoveMemberOfProjectCommandRequest(id.ToString(), memberId.ToString());
        await _sender.Send(request, token);

        await _hubContext.Clients
            .Group(id.ToString()).SendAsync("RemoveMemberProject", id, memberId, token);

        return Result.Ok();
    }


    [HttpDelete("{id:guid:required}")]
    public async Task<SuccessResponse> Remove(Guid id
        , CancellationToken token = default)
    {
        var request = new DeleteProjectCommandRequest(id.ToString());
        await _sender.Send(request, token);

        await _hubContext.Clients
            .Group(id.ToString()).SendAsync("RemoveProject", id.ToString(), token);

        return Result.Ok();
    }

}
