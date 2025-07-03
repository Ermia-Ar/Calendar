

using System.ComponentModel.DataAnnotations;

namespace Calendar.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(ISender sender) : ControllerBase
{
    private ISender _sender = sender;


    /// <summary>
    /// ثیت نام 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="token"></param>
    /// <returns></returns>
	[HttpPost("Register")]
    public async Task<SuccessResponse> Register([FromBody] RegisterCommandRequest request
        , CancellationToken token = default)
    {
        await _sender.Send(request, token);

        return Result.Ok();
    }

    /// <summary>
    /// ورودی کاربر
    /// </summary>
    /// <param name="request"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    [HttpPost("Login")]
    public async Task<SuccessResponse<string>> Login([FromBody] LoginCommandRequest request
        , CancellationToken token = default)
    {
        var result = await _sender.Send(request, token);

        return Result.Ok(result);
    }

    /// <summary>
    /// دریافت کاربر با ایدی
    /// </summary>
    /// <param name="id"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    [HttpGet("{id:guid:required}")]
    [Authorize]
    public async Task<SuccessResponse<GetUserByIdQueryResponse>> GetById(Guid id,
        CancellationToken token = default)
    {
        var request = new GetUserByIdQueryRequest(id.ToString());
        var result = await _sender.Send(request, token);

        return Result.Ok(result);
    }

    /// <summary>
    /// دریافت کاربر با نام کاربری
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    [HttpGet("{userName:required}")]
    [Authorize]
    public async Task<SuccessResponse<GetUserByUserNameQueryResponse>> GetByUserName([MinLength(3)] string userName
        , CancellationToken token = default)
    {
        var request = new GetUserByUserNameQueryRequest(userName);
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
    [Authorize]
    public async Task<SuccessResponse<PaginationResult<List<GetAllUserQueryResponse>>>> GetAll([FromQuery]GetAllUsersDto model
        , CancellationToken token = default)
    {
        var request = GetAllUsersQueryRequest.Create(model);
        var result = await _sender.Send(request, token);

        return Result.Ok(result);
    }
}
