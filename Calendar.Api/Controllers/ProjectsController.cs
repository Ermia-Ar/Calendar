namespace Calendar.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]

public class ProjectsController(ISender sender
    , IHubContext<CommonHub> hubContext
    , ICurrentUserServices currentUserServices
    ) : ControllerBase
{

    private ISender _sender = sender;
    private readonly IHubContext<CommonHub> _hubContext = hubContext;
    private readonly ICurrentUserServices _currentUserServices = currentUserServices;


    /// <summary>
    /// ساخت پروژه
    /// </summary>
    /// <param name="request"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<SuccessResponse> Add([FromBody] AddProjectCommandRequest request
        , CancellationToken token = default)
    {
        var requests = await _sender.Send(request, token);

        foreach(var memberId in request.MemberIds)
        {
            await _hubContext.Clients
                .User(memberId).SendAsync("RequestReceive", requests[memberId], token);
        }

        return Result.Ok();
    }

    /// <summary>
    /// ارسال درخواست عضویت در پروژه
    /// </summary>
    /// <remarks>
    /// کاربر فرستنده درخواست باید سازنده پروژه باشد
    /// </remarks>
    /// <param name="request"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    [HttpPost("SubmitRequest")]
    public async Task<SuccessResponse> SendProjectRequest([FromBody] SubmitProjectRequestCommandRequest request
        , CancellationToken token = default)
    {
		var requests = await _sender.Send(request, token);

        await _hubContext.Clients
            .Users(request.MemberIds).SendAsync("RequestReceive", token);

		foreach (var memberId in request.MemberIds)
		{
			await _hubContext.Clients
				.User(memberId).SendAsync("RequestReceive", requests[memberId], token);
		}

		return Result.Ok();
    }

	/// <summary>
	/// اضافه کردن یک فعالیت به پروژه
	/// </summary>
	/// <remarks>
	/// کاربر فرستده درخواست باید عضوی از پروژه باشه
	/// اگر فیلد نوتیفیکیشن نال ارسال شود اعلان پیش فرض برای کاربر ثبت میشود
	/// </remarks>
	/// <param name="request"></param>
	/// <param name="token"></param>
	/// <returns></returns>
	[HttpPost("Activities")]
    public async Task<SuccessResponse> AddActivity([FromBody] AddActivityForProjectCommandRequest request
        , CancellationToken token = default)
    {
        var requests = await _sender.Send(request, token);

		foreach (var memberId in requests.Keys)
		{
			await _hubContext.Clients
				.User(memberId).SendAsync("RequestReceive", requests[memberId], token);
		}

		return Result.Ok();
    }

    /// <summary>
    /// دربافت عضوهای یک پروژه
    /// </summary>
    /// <remarks>
    /// کاربر فرستنده درخواست باید عضوی از پروژه باشه
    /// </remarks>
    /// <param name="id"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    [HttpGet("Members/{id:guid}")]
    public async Task<SuccessResponse<List<GetMemberOfProjectQueryResponse>>> GetMembers([FromRoute] Guid id
        , CancellationToken token = default)
    {
        var request = new GetMemberOfProjectQueryRequest(id.ToString());
        var result = await _sender.Send(request, token);


        return Result.Ok(result);
    }

    /// <summary>
    /// دریافت تمام پروژه مربوط به کاربر
    /// </summary>
    /// <param name="model"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<SuccessResponse<PaginationResult<List<GetAllProjectQueryResponse>>>> GetAll([FromQuery] GetAllProjectDto model
        , CancellationToken token = default)
    {
        var request = GetAllProjectsQueryRequest.Create(model);
        var result = await _sender.Send(request, token);

        return Result.Ok(result);
    }

    /// <summary>
    /// دریافت پروژه با ایدی
    /// </summary>
    /// <param name="id"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    [HttpGet("{id:guid:required}")]
    public async Task<SuccessResponse<GetProjectByIdQueryResponse>> GetById(Guid id
        , CancellationToken token = default)
    {
        var request = new GetProjectByIdQueryRequest(id.ToString());
        var result = await _sender.Send(request, token);

        return Result.Ok(result);
    }

    /// <summary>
    /// حارج شدن از یک پروژه
    /// </summary>
    /// کاربر باید غضو پروژه باشه 
    /// با ارسال این درخواست علاوه بر این پروژه از تمام فعالیت های مربوط  به این پروژه نیز خارج می شوید
    /// <param name="id"></param>
    /// <param name="token"></param>
    /// <returns></returns>
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

	/// <summary>
	/// بیرون کردن یک عضو
	/// </summary>
	/// <remarks>
	/// کاربر فرستده درخواست باید سازنده پروژه باشد
	/// با ارسال این درخواست علاوه بر این پروژه از تمام فعالیت های مربوط  به این پروژه نیز بیرون می شود
	/// </remarks>
	/// <param name="id"></param>
	/// <param name="memberId"></param>
	/// <param name="token"></param>
	/// <returns></returns>
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

    /// <summary>
    /// حذف یک پروژه
    /// </summary>
    /// <remarks>
    /// کاربر فرستنده درخواست باید سازنده پروژه باشد
    /// علاوه بر پروژه تمام فعالیت ها درخواست ها و کامنت های مربوط به این پروژه نیز خذف می شوند
    /// </remarks>
    /// <param name="id"></param>
    /// <param name="token"></param>
    /// <returns></returns>
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
