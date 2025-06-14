
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

    //[HttpGet]
    //[Route("GetRequestsReceived")]
    ////[Authorize(CalendarClaims.GetRequestsReceived)]
    //public async Task<SuccessResponse<List<RequestResponse>>> GetRequestsReceived(RequestFor? requestFor)
    //{
    //    var request = new GetRequestsReceivedQuery (requestFor);
    //    var result = await _sender.Send(request);
    //    return Result.Ok(result);
    //}

    //[HttpGet]
    //[Route("GetRequestsResponse")]
    ////[Authorize(CalendarClaims.GetRequestsResponse)]
    //public async Task<SuccessResponse<List<RequestResponse>>> GetRequestsResponse(RequestFor? requestFor)
    //{
    //    var request = new GetRequestsResponseQuery (requestFor);
    //    var result = await _mediator.Send(request);
    //    return Result.Ok(result);
    //}

    //[HttpGet]
    //[Route("GetUnAnsweredRequest")]
    ////[Authorize(CalendarClaims.GetUnAnsweredRequest)]
    //public async Task<SuccessResponse<List<RequestResponse>>> GetUnAnsweredRequest(RequestFor? requestFor)
    //{
    //    var request = new GetUnAnsweredRequestQuery (requestFor);
    //    var result = await _mediator.Send(request);
    //    return Result.Ok(result);
    //}

    //[HttpGet]
    //[Route("GetResponsesUserSent")]
    ////[Authorize(CalendarClaims.GetResponsesUserSent)]
    //public async Task<SuccessResponse<List<RequestResponse>>> GetResponsesUserSent(RequestFor? requestFor)
    //{
    //    var request = new GetResponsesUserSentQuery (requestFor);
    //    var result = await _mediator.Send(request);
    //    return Result.Ok(result);
    //}
}
