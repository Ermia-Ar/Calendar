

namespace Calendar.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]

public class CommentsController(ISender sender, IHubContext<CommonHub> hubContext) : ControllerBase
{
	private readonly ISender _sender = sender;
	private readonly IHubContext<CommonHub> _hubContext = hubContext;


	/// <summary>
	/// کامنت گذاری برای فعالیت
	/// </summary>
	/// <param name="request"></param>
	/// <param name="token"></param>
	/// <returns></returns>
	[HttpPost]
	public async Task<SuccessResponse> Post(AddCommentCommandRequest request,
		CancellationToken token = default)
	{
	   var comment = await _sender.Send(request, token);

		await _hubContext.Clients
			.Group(request.ActivityId).SendAsync("PostComment", request, token);

		return Result.Ok();
	}

	/// <summary>
	/// به روزرسانی کامنت
	/// </summary>
	/// <remarks>
	/// کاربر فرستنده درخواست باید سازنده کامنت باشد
	/// </remarks>
	/// <param name="id">ایدی کامنت</param>
	/// <param name="content">متن کامنت</param>
	/// <param name="token"></param>
	/// <returns></returns>
	[HttpPut("{id:guid:required}")]
	public async Task<SuccessResponse> Put(Guid id, string content,
		CancellationToken token = default)
	{
		var request = new UpdateCommentCommandRequest(id.ToString(), content);
	    var comment = await _sender.Send(request, token);

		await _hubContext.Clients
			.Group(comment.ActivityId).SendAsync("UpdateComment", comment, token);

		return Result.Ok();
	}

	/// <summary>
	/// حذف کامنت
	/// </summary>	
	/// <remarks>
	/// کاربر فرستنده درخواست باید سازنده کامنت باشد
	/// </remarks>
	/// <param name="id">ایدی کامنت مورد نظر</param>
	/// <param name="token"></param>
	/// <returns></returns>
	[HttpDelete("{id:guid:required}")]
	public async Task<SuccessResponse> Remove(Guid id,
		CancellationToken token = default)
	{
		var request = new DeleteCommentCommandRequest(id.ToString());
		var activityId =  await _sender.Send(request, token);

		//send to group of activity
		await _hubContext.Clients.
			Group(activityId).SendAsync("RemoveComment", id.ToString(), token);

		return Result.Ok();
	}

	/// <summary>
	/// دریافت کامنت با ایدی
	/// </summary>
	/// <param name="id"></param>
	/// <param name="token"></param>
	/// <returns></returns>
	[HttpGet("{id:guid:required}")]
	public async Task<SuccessResponse<GetCommentByIdQueryResponse>> GetById(Guid id,
		CancellationToken token = default)
	{
		var request = new GetCommentByIdQueryRequest(id.ToString());
		var result = await _sender.Send(request, token);
		return Result.Ok(result);
	}

	/// <summary>
	/// دریافت تمامی کامنت های برنامه
	/// </summary>
	/// <remarks>
	/// انجام شود در غیر این صورت برنامه ارور می دهد userId, projectId, ActivityId حتما باید یکی از فیلتر ها 
	/// </remarks>
	/// <param name="model"></param>
	/// <param name="token"></param>
	/// <returns></returns>
	[HttpGet]
	public async Task<SuccessResponse<PaginationResult<List<GetAllCommentsQueryResponse>>>> GetAll([FromQuery] GetAllCommentDto model,
		CancellationToken token = default)
	{
		var request = GetAllCommentsQueryRequest.Create(model);
		var result = await _sender.Send(request, token);
		return Result.Ok(result);
	}
}
