using Core.Application.ApplicationServices.Auth.Queries.GetByUserName;
using Microsoft.AspNetCore.Authorization;

namespace Calendar.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private ISender _sender;

    public AuthController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost("Register")]
    public async Task<SuccessResponse> Register([FromBody] RegisterCommandRequest request
        , CancellationToken token = default)
    {
        await _sender.Send(request, token);

        return Result.Ok();
    }

    [HttpPost("Login")]
    public async Task<SuccessResponse<string>> Login([FromBody] LoginCommandRequest request
        , CancellationToken token = default)
    {
        var result = await _sender.Send(request, token);

        return Result.Ok(result);
    }

    [HttpGet("{id:guid:required}")]
    [Authorize]
    //[Authorize(CalendarClaims.GetUserByUserName)]
    public async Task<SuccessResponse<GetUserByIdQueryResponse>> GetById(Guid id,
        CancellationToken token = default)
    {
        var request = new GetUserByIdQueryRequest(id.ToString());
        var result = await _sender.Send(request, token);

        return Result.Ok(result);
    }

    [HttpGet("{userName:required}")]
    [Authorize]
    public async Task<SuccessResponse<GetUserByUserNameQueryResponse>> GetByUserName(string userName
        , CancellationToken token)
    {
        var request = new GetUserByUserNameQueryRequest(userName);
        var result = await _sender.Send(request, token);

        return Result.Ok(result);   
    }

    [HttpGet]
    [Authorize]
    //[Authorize(CalendarClaims.GetAllUsers)]
    public async Task<SuccessResponse<List<GetAllUserQueryResponse>>> GetAll([FromQuery]GetAllUsersDto model
        , CancellationToken token = default)
    {
        var request = GetAllUsersQueryRequest.Create(model);
        var result = await _sender.Send(request, token);

        return Result.Ok(result);
    }
}
