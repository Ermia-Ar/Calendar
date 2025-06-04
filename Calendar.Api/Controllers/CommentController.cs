using Core.Application.ApplicationServices.Comments.Queries.GetComments;
using Core.Application.Features.Comments.Commands.CreateComment;
using Core.Application.Features.Comments.Commands.DeleteComment;
using Core.Application.Features.Comments.Commands.UpdateComment;
using Core.Application.Features.Comments.Queries.GetCommentById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;
using SharedKernel.ResponseResult;

namespace Calendar.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CommentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CommentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("CreateComment")]
        //[Authorize(CalendarClaims.CreateComment)]
        public async Task<SuccessResponse> CreateComment(Guid projectId, string activityId, string content)
        {
            var request = new CreateCommentCommand(projectId.ToString(), activityId, content);
            var result = await _mediator.Send(request);

            return Result.Ok();
        }

        [HttpGet]
        [Route("GetComments")]
        //[Authorize(CalendarClaims.GetComments)]
        public async Task<SuccessResponse<List<GetCommentsQueryResponse>>> 
            GetComments(Guid? projectId, Guid? activityId, string? search, bool isUserOwner)
        {
            var request = new GetCommentsQueryRequest(projectId.ToString(), activityId.ToString(), search, isUserOwner);
            var result = await _mediator.Send(request);

            return Result.Ok(result);
        }

        [HttpGet]
        [Route("GetCommentById{id:guid}")]
        public async Task<SuccessResponse<GetCommentsQueryResponse>> GetCommentById(Guid id)
        {
            var request = new GetCommentByIdQuery(id.ToString());
            var result = await _mediator.Send(request);

            return Result.Ok(result);
        }

        [HttpPut]
        [Route("EditComment")]
        //[Authorize(CalendarClaims.UpdateComment)]
        public async Task<SuccessResponse> UpdateComment(Guid id, string content)
        {
            var request = new UpdateCommentCommand(id.ToString(), content);
            var result = await _mediator.Send(request);

            return Result.Ok();
        }

        [HttpDelete]
        [Route("DeleteComment")]
        //[Authorize(CalendarClaims.DeleteComment)]
        public async Task<SuccessResponse> DeleteComment(Guid id)
        {
            var request = new DeleteCommentCommand(id.ToString());
            var result = await _mediator.Send(request);
            return Result.Ok();
        }
    }
}
