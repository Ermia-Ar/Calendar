
using Calendar.Api.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Calendar.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]

public class CommentsController(ISender sender, IHubContext<CommonHub> hubContext) : ControllerBase
{
	private readonly ISender _sender = sender;
	private readonly IHubContext<CommonHub> _hubContext = hubContext;

	[HttpPost]
	public async Task<SuccessResponse> Post(AddCommentCommandRequest request,
		CancellationToken token = default)
	{
		await _sender.Send(request, token);

		await _hubContext.Clients
			.Group(request.ActivityId).SendAsync("PostComment", request);

		return Result.Ok();
	}


	[HttpPut("{id:guid:required}")]
	public async Task<SuccessResponse> Put(Guid id, string content,
		CancellationToken token = default)
	{
		var request = new UpdateCommentCommandRequest(id.ToString(), content);
		await _sender.Send(request, token);
		return Result.Ok();
	}


	[HttpDelete("{id:guid:required}")]
	public async Task<SuccessResponse> Remove(Guid id,
		CancellationToken token = default)
	{
		var request = new DeleteCommentCommandRequest(id.ToString());
		await _sender.Send(request, token);

		//send to group of activity
		//await _hubContext.Clients.
		//	Group().SendAsync("RemoveComment", token);

		return Result.Ok();
	}


	[HttpGet("{id:guid:required}")]
	public async Task<SuccessResponse<GetCommentByIdQueryResponse>> GetById(Guid id,
		CancellationToken token = default)
	{
		var request = new GetCommentByIdQueryRequest(id.ToString());
		var result = await _sender.Send(request, token);
		return Result.Ok(result);
	}


	[HttpGet]
	public async Task<SuccessResponse<List<GetAllCommentsQueryResponse>>> GetAll([FromQuery] GetAllCommentDto model,
		CancellationToken token = default)
	{
		var request = GetAllCommentsQueryRequest.Create(model);
		var result = await _sender.Send(request, token);
		return Result.Ok(result);
	}
}
