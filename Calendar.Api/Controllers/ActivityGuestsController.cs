using Calendar.Api.Base;
using Core.Application.Features.ActivityGuests.Commands;
using Core.Application.Features.ActivityGuests.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Calendar.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ActivityGuestsController : AppControllerBase
    {
        private IMediator _mediator;

        public ActivityGuestsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpDelete]
        [Route("ExitingFromActivity")]
        public async Task<IActionResult> ExitingFromActivity(Guid activityId)
        {
            var request = new ExitingFromActivityCommand { ActivityId = activityId.ToString() };
            var result = await _mediator.Send(request);

            return NewResult(result);
        }

        [HttpDelete]
        [Route("RemoveGuestFromActivity")]
        public async Task<IActionResult> RemoveGuestFromActivity(Guid activityId, Guid userId)
        {
            var request = new RemoveGuestFromActivityCommand { ActivityId = activityId.ToString(), UserId = userId.ToString() };
            var result = await _mediator.Send(request);

            return NewResult(result);
        }

        [HttpGet]
        [Route("GetActivityGuests")]
        public async Task<IActionResult> GetActivityGuests(string activityId)
        {
            var request = new GetActivityGuestByActivityIdQuery { ActivityId = activityId.ToString() };
            var result = await _mediator.Send(request);

            return NewResult(result);
        }

        [HttpGet]
        [Route("GetUserActivityGuests")]
        public async Task<IActionResult> GetUserActivityGuests()
        {
            var request = new GetUserActivityGuestQuery();
            var result = await _mediator.Send(request);

            return NewResult(result);
        }
    }
}
