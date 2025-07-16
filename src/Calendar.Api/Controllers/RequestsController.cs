
using Calendar.Api.Common;

namespace Calendar.Api.Controllers;	

[Route("api/[controller]")]
[ApiController]
public class RequestsController(ISender sender) : ControllerBase
{
	private readonly ISender _sender = sender;

	/// <summary>
	/// جواب دادن به درخواست
	/// </summary>
	/// <remarks>
	/// کاربر فرستنده درخواست باید دربافت کننده درخواست باشه
	/// </remarks>
	/// <param name="id"></param>
	/// <param name="model"></param>
	/// <param name="token"></param>
	[HttpPost("{id:long:required}/Answer")]
	[Authorize(CalendarClaimsServiceDeclaration.AnswerRequest)]
	public async Task<SuccessResponse> Answer(long id ,[FromQuery] AnswerRequestDto model,
		CancellationToken token = default)
	{
		var request = AnswerRequestCommandRequest.Create(id, model);
		await _sender.Send(request, token);
		return Result.Ok();
	}

	/// <summary>
	/// حذف درخواست 
	/// </summary>
	/// <remarks>
	/// کاربر فرستنده درخواست باید یا دریافت کننده یا فرستنده درخواست باشه
	/// </remarks>
	[HttpDelete("{id:long:required}")]
	[Authorize(CalendarClaimsServiceDeclaration.DeleteRequest)]
	public async Task<SuccessResponse> Remove([FromRoute] long id, CancellationToken token = default)
	{
		var request = new DeleteRequestCommandRequest(id);
		await _sender.Send(request);
		return Result.Ok();
	}

	/// <summary>
	/// دریافت درخواست با ایدی 
	/// </summary>
	[HttpGet("{id:long:required}")]
	[Authorize(CalendarClaimsServiceDeclaration.GetRequestById)]
	public async Task<SuccessResponse<GetRequestByIdQueryResponse>> GetById(long id, CancellationToken token)
	{
		var request = new GetRequestByIdQueryRequest(id);
		var result = await _sender.Send(request, token);
		return Result.Ok(result);
	}

	/// <summary>
	/// دریافت تمام درخواست های برنامه 
	/// </summary>
	/// <remarks>دارای صفحه بندی</remarks>
	/// <param name="model"></param>
	/// <param name="token"></param>
	[HttpGet]
	[Authorize(CalendarClaimsServiceDeclaration.GetAllRequests)]
	public async Task<SuccessResponse<PaginationResult<List<GetAllRequestQueryResponse>>>> GetAll([FromQuery] GetAllRequestDto model,
		CancellationToken token)
	{
		var request = GetAllRequestsQueryRequest.Create(model);
		var result = await _sender.Send(request, token);
		return Result.Ok(result);
	}
}
