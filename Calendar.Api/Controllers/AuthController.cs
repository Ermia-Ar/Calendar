
namespace Calendar.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("Register")]
    public async Task<SuccessResponse> Register([FromBody] RegisterCommandRequest request
        , CancellationToken token = default)
    {
        await _mediator.Send(request, token);

        return Result.Ok();
    }

    [HttpPost("Login")]
    public async Task<SuccessResponse> Login([FromBody] LoginCommandRequest request
        , CancellationToken token = default)
    {
        await _mediator.Send(request, token);

        return Result.Ok();
    }

    [HttpGet("{id:guid:required}")]
    //[Authorize(CalendarClaims.GetUserByUserName)]
    public async Task<SuccessResponse<GetUserByIdQueryResponse>> GetById(Guid id,
        CancellationToken token = default)
    {
        var request = new GetUserByIdQueryRequest(id.ToString());
        var result = await _mediator.Send(request, token);

        return Result.Ok(result);
    }

    [HttpGet]
    //[Authorize(CalendarClaims.GetAllUsers)]
    public async Task<SuccessResponse<List<GetAllUserQueryResponse>>> GetAll(string? search , UserCategory? category
        , CancellationToken token = default)
    {
        var request = new GetAllUsersQueryRequest(search, category);
        var result = await _mediator.Send(request, token);

        return Result.Ok(result);
    }
}
