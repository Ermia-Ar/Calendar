using Amazon.Runtime.Internal.Auth;
using Core.Activities.ApplicationServices.Activities.Queries.GetComments;
using Core.Application.ApplicationServices.Activities.Commands.AddRecurring;
using Core.Application.ApplicationServices.Activities.Commands.ExitingActivity;
using Core.Application.ApplicationServices.Activities.Commands.UpdateNotification;
using Core.Application.ApplicationServices.Activities.Commands.UpdateStartDate;
using Core.Application.ApplicationServices.Activities.Queries.GetComments;
using Core.Domain.Exceptions;

namespace Calendar.Api.Controllers;

[Route("api/[controller]/")]
[ApiController]
public class ActivitiesController(ISender sender
	) : ControllerBase
{
	private readonly ISender _sender = sender;


	/// <summary>
	/// ساخت فعالیت با پروژه ایدی پیش فرض
	/// </summary>
	/// <remarks>
	/// اگر فیلد نوتیفیکیشن نال ارسال شود اعلان پیش فرض برای کاربر ثبت میشود
	/// </remarks>
	/// <param name="request">request for new activity</param>
	/// <param name="token"></param>
	/// <returns></returns>
	[HttpPost]
	[Authorize(CalendarClaimsServiceDeclaration.CreateActivity)]
	public async Task<SuccessResponse> Post([FromBody] AddActivityCommandRequest request
		, CancellationToken token = default)
	{
		await _sender.Send(request, token);

		return Result.Ok();
	}

	/// <summary>
	/// ساخت فعالیت دوره ای
	/// </summary>
	/// <param name="request"></param>
	/// <param name="token"></param>
	/// <returns></returns>
	[HttpPost("Recurring")]
	public async Task<SuccessResponse> PostRecurring(AddRecurringActivityCommnadRequest request,
		CancellationToken token)
	{
		await _sender.Send(request, token);

		return Result.Ok();
	}

	/// <summary>
	/// ساخت دنباله برای فعالیت 
	/// </summary>
	/// <remarks>
	/// فقط سازده فعالیت اجازه ساخت دنباله برای آن را دارد 
	/// اگر فیلد نوتیفیکیشن نال ارسال شود اعلان پیش فرض برای کاربر ثبت میشود
	/// </remarks>
	/// <param name="id"></param>
	/// <param name="model"></param>
	/// <param name="token"></param>
	/// <returns></returns>
	[HttpPost("{id:long:required}/SubActivity")]
	[Authorize(CalendarClaimsServiceDeclaration.CreateSubActivity)]
	public async Task<SuccessResponse> PostSubActivity([FromRoute] long id, [FromBody] AddSubActivityDto model
		 , CancellationToken token = default)
	{
		var request = AddSubActivityCommandRequest.Create(id, model);
		await _sender.Send(request, token);

		return Result.Ok();
	}

	/// <summary>
	/// ارسال درخواست برای عضویت
	/// </summary>
	/// <remarks>
	/// .فقط سازده فعالیت اجازه ارسال درخواست برای آن را دارد 
	/// نباید از قبل برای دربافت کننده چنین درخواستی ثیت شده باشد
	/// </remarks>
	/// <param name="id"></param>
	/// <param name="model"></param>
	/// <param name="token"></param>
	/// <returns></returns>
	[HttpPost("Request")]
	[Authorize(CalendarClaimsServiceDeclaration.SendJoinRequest)]
	public async Task<SuccessResponse> SendRequest(long id, [FromBody] SubmitActivityRequestDto model
		, CancellationToken token = default)
	{
		var request = SubmitActivityRequestCommandRequest.Create(id, model);
		await _sender.Send(request, token);

		return Result.Ok();
	}

	/// <summary>
	/// دریافت تمام فعالیت های کاربر فعلی
	/// </summary>
	/// <remarks>
	/// دارای صفحه بندی
	/// </remarks>
	/// <param name="model"></param>
	/// <param name="token"></param>
	/// <returns></returns>
	[HttpGet]
	[Authorize(CalendarClaimsServiceDeclaration.GetAllActivities)]
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
	/// <param name="model"></param>
	/// <returns></returns>
	[HttpGet("{id:long}/Members")]
	[Authorize(CalendarClaimsServiceDeclaration.GetActivityMembers)]
	public async Task<SuccessResponse<PaginationResult<List<GetMemberOfActivityQueryResponse>>>> GetMember(long id,
		 [FromQuery] GetMemberOfActivityDto model
		, CancellationToken token = default)
	{
		var request = GetMemberOfActivityQueryRequest.Create(id, model);
		var result = await _sender.Send(request, token);

		return Result.Ok(result);
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="id"></param>
	/// <param name="model"></param>
	/// <param name="token"></param>
	/// <returns></returns>
	[HttpGet("{id:long:required}/Comments")]
	public async Task<SuccessResponse<PaginationResult<List<GetActivityCommentsQueryResponse>>>> GetCommnets(long id,
		[FromQuery] GetActivityCommentsDto model,
		CancellationToken token)
	{
		var request = GetActivityCommentsQueryRequest.Create(id, model);
		var response = await _sender.Send(request, token);

		return	Result.Ok(response);
	}

	/// <summary>
	/// دریافت یک فعالیت با ایدی آن 
	/// </summary>
	/// <param name="id">ایدی فعالیت</param>
	/// <param name="token"></param>
	/// <returns></returns>
	[HttpGet("{id:long:required}")]
	[Authorize(CalendarClaimsServiceDeclaration.GetActivityById)]
	public async Task<SuccessResponse<GetActivityByIdQueryResponse>> GetById(long id
		, CancellationToken token = default)
	{
		var request = new GetActivityByIdQueryRequest(id);
		var result = await _sender.Send(request, token);

		return Result.Ok(result);
	}

	/// <summary>
	/// به روزرسانی کلی یک فعالیت
	/// </summary>
	/// <param name="id"></param>
	/// <param name="model"></param>
	/// <param name="token"></param>
	/// <returns></returns>
	[HttpPut("{id:long:required}")]
	[Authorize(CalendarClaimsServiceDeclaration.UpdateActivity)]
	public async Task<SuccessResponse> Put(long id, UpdateActivityDto model
	 , CancellationToken token = default)
	{
		var request = UpdateActivityCommandRequest.Create(id, model);
		await _sender.Send(request, token);

		return Result.Ok();
	}

	/// <summary>
	/// تکمیل یک فعالیت
	/// </summary>
	/// <remarks>
	/// کاربری که این درخواست رو ارسال میکنه باید سازنده فعالیت باشه
	/// </remarks>
	/// <param name="id"></param>
	/// <param name="token"></param>
	/// <returns></returns>
	[HttpPatch("{id:long:required}/Complete")]
	[Authorize(CalendarClaimsServiceDeclaration.CompleteActivity)]
	public async Task<SuccessResponse> Complete(long id
		, CancellationToken token = default)
	{
		var request = new CompleteActivityCommandRequest(id);
		await _sender.Send(request, token);


		return Result.Ok();
	}

	/// <summary>
	/// تغییر در زمان ارسال اعلان
	/// </summary>
	/// <remarks>
	/// کاربر باید عضوی از فعالیت باشد
	/// اگر فیلد نوتیفیکیشن نال ارسال شود دیگر اعلانی برای کاربر فعلی ارسال نمی شود
	/// </remarks>
	/// <param name="id"></param>
	/// <param name="model"></param>
	/// <param name="token"></param>
	/// <returns></returns>
	[HttpPatch("{id:long:required}/Notification")]
	[Authorize(CalendarClaimsServiceDeclaration.ChangeNotificationTime)]
	public async Task<SuccessResponse> Notification(long id, UpdateNotificationDto model
		, CancellationToken token = default)
	{
		var request = UpdateNotificationCommandRequest.Create(id, model);
		await _sender.Send(request, token);

	 	return Result.Ok();
	}

	/// <summary>
	/// تغییر در زمان شروع یک فعالیت
	/// </summary>
	/// <remarks>
	/// کاربری که این درخواست رو ارسال میکنه باید سازنده فعالیت باشه
	/// </remarks>
	/// <param name="id"></param>
	/// <param name="model"></param>
	/// <param name="token"></param>
	/// <returns></returns>
	[HttpPatch("{id:long:required}/StartDate")]
	[Authorize(CalendarClaimsServiceDeclaration.ChangeStartDate)]

	public async Task<SuccessResponse> ChangeStartDate(long id, UpdateActivityStartDateDto model
		, CancellationToken token = default)
	{
		var request = UpdateActivityStartDateCommandRequest.Create(id, model);
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
	[HttpDelete("{id:long:required}")]
	//[Authorize(CalendarClaimsServiceDeclaration.DeleteActivity)]
	public async Task<SuccessResponse> Remove([FromRoute] long id
		, CancellationToken token = default)
	{
		var request = new DeleteActivityCommandRequest(id);
		await _sender.Send(request, token);
		

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
	[HttpDelete("{id:long:required}/Exiting")]
	[Authorize(CalendarClaimsServiceDeclaration.LeaveActivity)]
	public async Task<SuccessResponse> Exiting(long id
		, CancellationToken token = default)
	{
		var request = new ExitingActivityCommandRequest(id);
		await _sender.Send(request, token);

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
	[HttpDelete("{id:long:required}/Member/{memberId:guid:required}")]
	[Authorize(CalendarClaimsServiceDeclaration.RemoveActivityMember)]
	public async Task<SuccessResponse> RemoveMember(long id, Guid memberId
		, CancellationToken token = default)
	{
		var request = new RemoveMemberOfActivityCommandRequest(id, memberId);
		await _sender.Send(request, token);

		return Result.Ok();
	}
}