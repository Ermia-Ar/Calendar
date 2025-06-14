
namespace Calendar.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CommentsController(ISender sender) : ControllerBase
{
    private readonly ISender _sender = sender;

    [HttpPost]
    //[Authorize(CalendarClaims.CreateComment)]
    public async Task<SuccessResponse> Post(AddCommentCommandRequest request,
        CancellationToken token = default)
    {
        await _sender.Send(request, token);
        return Result.Ok();
    }

    [HttpPut("{id:guid:required}")]
    //[Authorize(CalendarClaims.UpdateComment)]
    public async Task<SuccessResponse> Put(Guid id, string content,
        CancellationToken token = default)
    {
        var request = new UpdateCommentCommandRequest(id.ToString(), content);
        await _sender.Send(request, token);
        return Result.Ok();
    }

    [HttpDelete("{id:guid:required}")]
    //[Authorize(CalendarClaims.DeleteComment)]
    public async Task<SuccessResponse> Remove(Guid id,
        CancellationToken token = default)
    {
        var request = new DeleteCommentCommandRequest(id.ToString());
        await _sender.Send(request, token);
        return Result.Ok();
    }

    [HttpGet("{id:guid:required}")]
    public async Task<SuccessResponse<GetCommentByIdQueryResponse>> GetById(Guid id,
        CancellationToken token = default)
    {
        var result = await _sender.Send(new GetCommentByIdQueryRequest(id.ToString()), token);
        return Result.Ok(result);
    }

    [HttpGet]
    //[Authorize(CalendarClaims.GetComments)]
    public async Task<SuccessResponse<List<GetCommentsQueryResponse>>> GetAll([FromQuery] GetAllCommentDto model,
        CancellationToken token = default)
    {
        var request = GetAllCommentsQueryRequest.Create(model);
        var result = await _sender.Send(request, token);
        return Result.Ok(result);
    }
}
