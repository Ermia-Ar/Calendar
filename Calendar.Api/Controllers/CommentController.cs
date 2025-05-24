using Calendar.Api.Base;
using Core.Application.Features.Comments.Commands.CreateComment;
using Core.Application.Features.Comments.Commands.DeleteComment;
using Core.Application.Features.Comments.Commands.UpdateComment;
using Core.Application.Features.Comments.Queries.GetComments;
using Core.Domain.Helper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Calendar.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CommentController : AppControllerBase
    {
        private readonly IMediator _mediator;

        public CommentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("CreateComment")]
        //[Authorize(CalendarClaims.CreateComment)]
        public async Task<IActionResult> CreateComment(Guid projectId, string activityId, string content)
        {
            var request = new CreateCommentCommand(projectId.ToString(), activityId, content);
            var result = await _mediator.Send(request);
            return NewResult(result);
        }

        [HttpGet]
        [Route("GetComments")]
        //[Authorize(CalendarClaims.GetComments)]
        public async Task<IActionResult> GetComments(Guid? projectId, Guid? activityId, string? search, bool isUserOwner)
        {
            var request = new GetCommentsQuery(projectId.ToString(), activityId.ToString(), search, isUserOwner);
            var result = await _mediator.Send(request);
            return NewResult(result);
        }

        [HttpPut]
        [Route("EditComment")]
        //[Authorize(CalendarClaims.UpdateComment)]
        public async Task<IActionResult> UpdateComment(Guid id, string content)
        {
            var request = new UpdateCommentCommand(id.ToString(), content);
            var result = await _mediator.Send(request);
            return NewResult(result);
        }

        [HttpDelete]
        [Route("DeleteComment")]
        //[Authorize(CalendarClaims.DeleteComment)]
        public async Task<IActionResult> DeleteComment(Guid id)
        {
            var request = new DeleteCommentCommand(id.ToString());
            var result = await _mediator.Send(request);
            return NewResult(result);
        }
    }
}
