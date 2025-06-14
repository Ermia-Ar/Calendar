
namespace Calendar.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProjectController : ControllerBase
{
    private ISender _sender;

    public ProjectController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    //[Authorize(CalendarClaims.CreateProject)]
    public async Task<SuccessResponse> Add([FromBody] AddProjectCommandRequest request
        , CancellationToken token = default)
    {
        await _sender.Send(request, token);

        return Result.Ok();
    }

    [HttpPost("SubmitRequest")]
    //[Authorize(CalendarClaims.SendProjectRequest)]
    public async Task<SuccessResponse> SendProjectRequest([FromBody] SubmitProjectRequestCommandRequest request
        , CancellationToken token = default)
    {
        await _sender.Send(request, token);

        return Result.Ok();
    }

    [HttpGet("GetMembers/{id:guid}")]
    //[Authorize(CalendarClaims.GetMemberOfProject)]
    public async Task<SuccessResponse<List<GetMemberOfProjectQueryResponse>>> GetMembers([FromRoute] Guid id
        , CancellationToken token = default)
    {
        var request = new GetMemberOfProjectQueryRequest(id.ToString());
        var result = await _sender.Send(request, token);

        return Result.Ok(result);
    }

    [HttpGet("GetActivities/{id:guid}")]
    //[Authorize(CalendarClaims.GetActivitiesOfProject)]
    public async Task<SuccessResponse<List<GetActivityOfProjectQueryResponse>>> GetActivities([FromRoute] Guid id
        , CancellationToken token = default)
    {
        var request = new GetActivitiesOfProjectQueryRequest(id.ToString());
        var result = await _sender.Send(request, token);

        return Result.Ok(result);
    }

    [HttpGet("GetAll")]
    //[Authorize(CalendarClaims.GetAllUserProjects)]
    public async Task<SuccessResponse<List<GetUserProjectQueryResponse>>> GetAll(DateTime? startDate, bool userIsOwner, bool isHistory
        , CancellationToken token = default)
    {
        var request = new GetUserProjectsQueryRequest(startDate, userIsOwner, isHistory);
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
    //Check
    [HttpDelete("Exiting/{id:guid:required}")]
    //[Authorize(CalendarClaims.ExitingProject)]
    public async Task<SuccessResponse> Exiting(Guid id
        , CancellationToken token = default)
    {
        var request = new ExitingProjectCommandRequest(id.ToString());
        await _sender.Send(request, token);

        return Result.Ok();
    }

    [HttpDelete("RemoveOf/{id:guid:required}/Member/{memberId:guid:required}")]
    //[Authorize(CalendarClaims.RemoveMemberOfProject)]
    public async Task<SuccessResponse> RemoveMember(Guid id, Guid memberId,
        CancellationToken token = default)
    {
        var request = new RemoveMemberOfProjectCommandRequest(id.ToString(), memberId.ToString());
        await _sender.Send(request, token);

        return Result.Ok();
    }

    [HttpDelete("{id:guid:required}")]
    //[Authorize(CalendarClaims.DeleteProject)]
    public async Task<SuccessResponse> Remove(Guid id
        , CancellationToken token = default)
    {
        var request = new DeleteProjectCommandRequest(id.ToString());
        await _sender.Send(request, token);

        return Result.Ok();
    }
}
