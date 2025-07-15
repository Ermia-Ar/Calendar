using Core.Application.InternalServices.Auth.Dtos;
using Core.Domain.Exceptions;

namespace Calendar.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(ISender sender) : ControllerBase
{
	private readonly ISender _sender = sender;

	/// <summary>
	/// ورودی کاربر
	/// </summary>
	/// <param name="request"></param>
	/// <param name="token"></param>
	/// <returns></returns>
	[HttpPost("Login")]
	[Authorize(CalendarClaimsServiceDeclaration.Login)]
	public async Task<SuccessResponse<LoginRequestResponse>> Login([FromBody] LoginCommandRequest request
		, CancellationToken token = default)
	{
		var result = await _sender.Send(request, token);

		return Result.Ok(result);
	}

	/// <summary>
	/// دریافت تمام کاربر های برنامه 
	/// </summary>
	/// <remarks>
	/// دارای صفحه بندی
	/// </remarks>
	/// <param name="model"></param>
	/// <param name="token"></param>
	/// <returns></returns>
	[HttpGet]
	[Authorize(CalendarClaimsServiceDeclaration.GetAllUsers)]
	public async Task<SuccessResponse<PaginationResult<List<GetAllUserQueryResponse>>>> GetAll([FromQuery] GetAllUsersDto model
		, CancellationToken token = default)
	{
		var request = GetAllUsersQueryRequest.Create(model);
		var result = await _sender.Send(request, token);

		return Result.Ok(result);
	}
}
