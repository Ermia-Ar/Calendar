using Core.Application.ApplicationServices.Activities.Commands.ExitingActivity;
using Core.Application.ApplicationServices.Activities.Commands.UpdateNotification;
using Core.Application.ApplicationServices.Activities.Commands.UpdateStartDate;

namespace Calendar.Api.Controllers;

[Route("api/[controller]/")]
[ApiController]
[Authorize]
public class ActivitiesController(ISender sender
	, IHubContext<CommonHub> hubContext
	, ICurrentUserServices userServices
	) : ControllerBase
{
	private readonly ISender _sender = sender;
	private readonly IHubContext<CommonHub> _hubContext = hubContext;
	private readonly ICurrentUserServices _userServices = userServices;


	/// <summary>
	/// ساخت فعالیت یا پروژه ایدی پیش فرض
	/// </summary>
	/// <remarks>
	/// اگر فیلد نوتیفیکیشن نال ارسال شود اعلان پیش فرض برای کاربر ثبت میشود
	/// </remarks>
	/// <param name="request">reqeust for new activity</param>
	/// <param name="token"></param>
	/// <returns></returns>
	[HttpPost]
	public async Task<SuccessResponse> Post([FromBody] AddActivityCommandRequest request
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
	/// ساخت دنباله برای فعالیت 
	/// </summary>
	/// <remarks>
	/// فقط سازده فعالیت اجازه ساخت دنباله برای آن را دارد 
	/// اگر فیلد نوتیفیکیشن نال ارسال شود اعلان پیش فرض برای کاربر ثبت میشود
	/// </remarks>
	/// <param name="request"></param>
	/// <param name="token"></param>
	/// <returns></returns>
	[HttpPost("SubActivity")]
	public async Task<SuccessResponse> PostSubActivity([FromBody] AddSubActivityCommandRequest request
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
	/// ارسال درخواست برای عضویت
	/// </summary>
	/// <remarks>
	/// .فقط سازده فعالیت اجازه ارسال درخواست برای آن را دارد 
	/// نباید از قبل برای دربافت کننده چنین درخواستی ثیت شده باشد
	/// </remarks>
	/// <param name="request"></param>
	/// <param name="token"></param>
	/// <returns></returns>
	[HttpPost("SubmitRequest")]
	public async Task<SuccessResponse> SendRequest([FromBody] SubmitActivityRequestCommandRequest request
		, CancellationToken token = default)
	{
		var requests =  await _sender.Send(request, token);

		foreach (var memberId in requests.Keys)
		{
			await _hubContext.Clients
				.User(memberId).SendAsync("RequestReceive", requests[memberId], token);
		}

		return Result.Ok();
	}

	/// <summary>
	/// دربافت تمام فعالیت های کاربر فعلی
	/// </summary>
	/// <param name="model"></param>
	/// <param name="token"></param>
	/// <returns></returns>
	[HttpGet]
	public async Task<SuccessResponse<PaginationResult<List<GetAllActivitiesQueryResponse>>>> GetAll([FromQuery] GetAllActivitiesDto model
		, CancellationToken token = default)
	{
		var request = GetAllActivitiesQueryRequest.Create(model);
		var result = await _sender.Send(request, token);

		return Result.Ok(result);
	}

	/// <summary>
	/// دریافت تمام عضو های که یک فعالیت
	/// </summary>
	/// <remarks>
	/// کاربری که این درخواست رو می فرستده باید عضویی از این فعالیت باشه
	/// </remarks>
	/// <param name="id">ایدی فعالیت</param>
	/// <param name="token"></param>
	/// <returns></returns>
	[HttpGet("Members/{id:guid}")]
	public async Task<SuccessResponse<List<GetMemberOfActivityQueryResponse>>> GetMember(Guid id
		, CancellationToken token = default)
	{
		var request = new GetMemberOfActivityQueryRequest(id.ToString());
		var result = await _sender.Send(request, token);

		return Result.Ok(result);
	}

	/// <summary>
	/// دریافت یک فعالیت با ایدی آن 
	/// </summary>
	/// <param name="id">ایدی فعالیت</param>
	/// <param name="token"></param>
	/// <returns></returns>
	[HttpGet("{id:guid}")]
	public async Task<SuccessResponse<GetActivityByIdQueryResponse>> GetById(Guid id
		, CancellationToken token = default)
	{
		var request = new GetActivityByIdQueryRequest(id.ToString());
		var result = await _sender.Send(request, token);

		return Result.Ok(result);
	}

	/// <summary>
	/// به روزرسانی کلی یک فعالیت
	/// </summary>
	/// <param name="request"></param>
	/// <param name="token"></param>
	/// <returns></returns>
	[HttpPut]
	public async Task<SuccessResponse> Put(UpdateActivityCommandRequest request
	 , CancellationToken token = default)
	{
		await _sender.Send(request, token);

		return Result.Ok();
	}

	/// <summary>
	/// تکمبل یک فعالیت
	/// </summary>
	/// <remarks>
	/// کاربری که این درخواست رو ارسال میکنه باید سازنده فعالیت باشه
	/// </remarks>
	/// <param name="id"></param>
	/// <param name="token"></param>
	/// <returns></returns>
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

	/// <summary>
	/// تغییر در زمان ارسال اعلان
	/// </summary>
	/// <remarks>
	/// کاربر باید عضوی از فعالیت باشد
	/// اگر فیلد نوتیفیکیشن نال ارسال شود دیگر اعلانی برای کاربر فعلی ارسال نمی شود
	/// </remarks>
	/// <param name="request"></param>
	/// <param name="token"></param>
	/// <returns></returns>
	[HttpPatch("Notification")]
	public async Task<SuccessResponse> Notification(UpdateNotificationCommandRequest request
		, CancellationToken token = default)
	{
		await _sender.Send(request, token);

	 	return Result.Ok();
	}

	/// <summary>
	/// تغییر در زمان شروع یک فعالیت
	/// </summary>
	/// <remarks>
	/// کاربری که این درخواست رو ارسال میکنه باید سازنده فعالیت باشه
	/// </remarks>
	/// <param name="request"></param>
	/// <param name="token"></param>
	/// <returns></returns>
	[HttpPatch("StartDate")]
	public async Task<SuccessResponse> ChangeStartDate(UpdateActivityStartDateCommandRequest request
		, CancellationToken token = default)
	{
		await _sender.Send(request, token);

		return Result.Ok();
	}

	/// <summary>
	/// حذف فعالیت
	/// </summary>
	/// <remarks>
	/// کاربری که این درخواست رو ارسال میکنه باید سازنده فعالیت باشه
	/// تمام کامنت ها و درخواست های مربوط به این فعالیت نیز حذف میشوند
	/// </remarks>
	/// <param name="id"></param>
	/// <param name="token"></param>
	/// <returns></returns>
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

	/// <summary>
	/// خارج شدن از یک فعالیت
	/// </summary>
	/// <remarks>
	/// کاربر باید عضوی از فعالیت باشد
	/// </remarks>
	/// <param name="id">ایدی فعالیت</param>
	/// <param name="token"></param>
	/// <returns></returns>
	[HttpDelete("Exiting/{id:guid:required}")]
	public async Task<SuccessResponse> Exiting(Guid id
		, CancellationToken token = default)
	{
		var request = new ExitingActivityCommandRequest(id.ToString());
		await _sender.Send(request, token);

		var userId = _userServices.GetUserId();
		await _hubContext.Clients
			 .Group(id.ToString()).SendAsync("ExitMemberActivity", id.ToString(), userId, token);

		return Result.Ok();
	}

	/// <summary>
	/// بیرون کردن یک عضو از فعالیت
	/// </summary>
	/// <remarks>
	/// کاربری که این درخواست رو ارسال میکنه باید سازنده فعالیت باشه
	/// </remarks>
	/// <param name="id">ایدی فعالیت</param>
	/// <param name="memberId">ایدی عضو</param>
	/// <param name="token"></param>
	/// <returns></returns>
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