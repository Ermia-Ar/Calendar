namespace Calendar.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]

public class RequestsController(ISender sender, IHubContext<CommonHub> hubContext) : ControllerBase
{
    private readonly ISender _sender = sender;
    private readonly IHubContext<CommonHub> _hubContext = hubContext;

    /// <summary>
    /// جواب به درخواست
    /// </summary>
    /// <remarks>
    /// کاربر فرستنده درخواست باید دربافت کننده درخواست باشه
    /// </remarks>
    /// <param name="request"></param>
    /// <param name="token"></param>
    /// <returns></returns>
	[HttpPost("Answer")]
    public async Task<SuccessResponse> Answer([FromQuery]AnswerRequestCommandRequest request
        , CancellationToken token = default)
    {
		var result = await _sender.Send(request, token);

        await _hubContext.Clients
            .User(result.SenderId).SendAsync("AnswerRequest", result.Id, result.Status, token);

        return Result.Ok();
    }

    /// <summary>
    /// حذف درخواست 
    /// </summary>
    /// <remarks>
    /// کاربر فرستنده درخواست باید یا دریافت کننده یا فرستنده درخواست باشه
    /// </remarks>
    /// <param name="id"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    [HttpDelete("{id:guid}")]
    public async Task<SuccessResponse> Remove([FromRoute] Guid id,
        CancellationToken token = default)
    {
        var request = new DeleteRequestCommandRequest(id.ToString());
        await _sender.Send(request);

        return Result.Ok();
    }
    /// <summary>
    /// دریافت درخواست با ایدی 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    [HttpGet("{id:guid:required}")]
    public async Task<SuccessResponse<GetRequestByIdQueryResponse>> GetById(Guid id
        , CancellationToken token)
    {
        var request = new GetRequestByIdQueryRequest(id.ToString());
        var result = await _sender.Send(request, token);

        return Result.Ok(result);
    }
    /// <summary>
    /// دریافت تمام درخواست های برنامه 
    /// </summary>
    /// <param name="model"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    [HttpGet()]
    public async Task<SuccessResponse<PaginationResult<List<GetAllRequestQueryResponse>>>> GetAll([FromQuery] GetAllRequestDto model
        , CancellationToken token)
    {
        var request = GetAllRequestsQueryRequest.Create(model);
        var result = await _sender.Send(request, token);   

        return Result.Ok(result);
    }

}
