using Core.Domain.Exceptions;

namespace Calendar.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CommentsController(ISender sender) : ControllerBase
{
	private readonly ISender _sender = sender;

	/// <summary>
	/// کامنت گذاری برای فعالیت
	/// </summary>
	/// <param name="request"></param>
	/// <param name="token"></param>
	/// <returns></returns>
	[HttpPost]
	[Authorize(CalendarClaimsServiceDeclaration.AddComment)]
	public async Task<SuccessResponse> Post(AddCommentCommandRequest request,
		CancellationToken token = default)
	{
		await _sender.Send(request, token);
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
	[HttpPut("{id:long:required}")]
	[Authorize(CalendarClaimsServiceDeclaration.UpdateComment)]
	public async Task<SuccessResponse> Put(long id, string content,
		CancellationToken token = default)
	{
		var request = new UpdateCommentCommandRequest(id, content);
		await _sender.Send(request, token);
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
	[HttpDelete("{id:long:required}")]
	[Authorize(CalendarClaimsServiceDeclaration.DeleteComment)]
	public async Task<SuccessResponse> Remove(long id,
		CancellationToken token = default)
	{
		var request = new DeleteCommentCommandRequest(id);
		await _sender.Send(request, token);
		return Result.Ok();
	}

	/// <summary>
	/// دریافت کامنت با ایدی
	/// </summary>
	/// <param name="id"></param>
	/// <param name="token"></param>
	/// <returns></returns>
	[HttpGet("{id:long:required}")]
	[Authorize(CalendarClaimsServiceDeclaration.GetCommentById)]
	public async Task<SuccessResponse<GetCommentByIdQueryResponse>> GetById(long id,
		CancellationToken token = default)
	{
		var request = new GetCommentByIdQueryRequest(id);
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
	[Authorize(CalendarClaimsServiceDeclaration.GetAllComments)]
	public async Task<SuccessResponse<PaginationResult<List<GetAllCommentsQueryResponse>>>> GetAll([FromQuery] GetAllCommentDto model,
		CancellationToken token = default)
	{
		var request = GetAllCommentsQueryRequest.Create(model);
		var result = await _sender.Send(request, token);
		return Result.Ok(result);
	}
}
