
using Core.Application.ApplicationServices.UserRequests.Queries.GetAll;
using Core.Application.ApplicationServices.UserRequests.Queries.GetById;

namespace Calendar.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserRequestsController : ControllerBase
{
    private ISender _sender;

    public UserRequestsController(ISender sender)
    {
        _sender = sender;
    }


    [HttpPost("Answer")]
    //[Authorize(CalendarClaims.AnswerRequest)]
    public async Task<SuccessResponse> Answer(AnswerRequestCommandRequest request
        , CancellationToken token = default)
    {
        await _sender.Send(request, token);

        return Result.Ok();
    }

    [HttpDelete("{id:guid}")]
    //[Authorize(CalendarClaims.RemoveRequest)]
    public async Task<SuccessResponse> Remove([FromRoute] Guid id,
        CancellationToken token = default)
    {
        var request = new DeleteRequestCommandRequest(id.ToString());
        await _sender.Send(request);

        return Result.Ok();
    }

    [HttpGet("{id:guid:required}")]
    public async Task<SuccessResponse<GetRequestByIdQueryResponse>> GetById(Guid id
        , CancellationToken token)
    {
        var request = new GetRequestByIdQueryRequest(id.ToString());
        var result = await _sender.Send(request, token);

        return Result.Ok(result);
    }

    [HttpGet("All")]
    public async Task<SuccessResponse<List<GetAllRequestQueryResponse>>> GetAll([FromQuery] GetAllRequestDto model
        , CancellationToken token)
    {
        var request = GetAllRequestsQueryRequest.Create(model);
        var result = await _sender.Send(request, token);   

        return Result.Ok(result);
    }

}
