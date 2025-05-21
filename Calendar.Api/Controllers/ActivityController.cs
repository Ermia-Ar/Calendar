using Calendar.Api.Base;
using Core.Application.DTOs.ActivityDTOs;
using Core.Application.Features.Activities.Commands;
using Core.Application.Features.Activities.Queries;
using Core.Domain.Enum;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Calendar.Api.Controllers
{
    [Route("api/[controller]/")]
    [ApiController]
    [Authorize]
    public class ActivityController : AppControllerBase
    {
        private IMediator _mediator;

        public ActivityController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("CreateActivity")]
        //[Authorize(CalendarClaims.CreateActivity)]
        public async Task<IActionResult> CreateActivity([FromBody] CreateActivityRequest activityRequest)
        {
            var request = new CreateActivityCommand (activityRequest);
            var result = await _mediator.Send(request);

            return NewResult(result);
        }
        
        [HttpPost]
        [Route("CreateActivityForProject")]
        //[Authorize(CalendarClaims.CreateActivityForProject)]
        public async Task<IActionResult> CreateActivityForProject([FromBody] CreateActivityForProjectRequest activityRequest)
        {
            var request = new CreateActivityForProjectCommand {CreateActivity = activityRequest };
            var result = await _mediator.Send(request);

            return NewResult(result);
        }
        
        [HttpPost]
        [Route("CreateSubActivity")]
        //[Authorize(CalendarClaims.CreateSubActivity)]
        public async Task<IActionResult> CreateSubActivity([FromBody] CreateSubActivityRequest activityRequest)
        {
            var request = new CreateSubActivityCommand { CreateActivity = activityRequest};
            var result = await _mediator.Send(request);

            return NewResult(result);
        }

        [HttpDelete]
        [Route("DeleteActivity{id:guid}")]
        //[Authorize(CalendarClaims.DeleteActivity)]
        public async Task<IActionResult> DeleteActivity([FromRoute] Guid id)
        {
            var request = new DeleteActivityCommand (id.ToString());
            var result = await _mediator.Send(request);

            return NewResult(result);
        }

        [HttpDelete]
        [Route("ExitingActivity{id:guid}")]
        //[Authorize(CalendarClaims.ExitingActivity)]
        public async Task<IActionResult> ExitingActivity([FromRoute] Guid id)
        {
            var request = new ExitingActivityCommand (id.ToString());
            var result = await _mediator.Send(request);

            return NewResult(result);
        }

        [HttpDelete]
        [Route("RemoveMemberOfActivity")]
        //[Authorize(CalendarClaims.RemoveMemberOfActivity)]
        public async Task<IActionResult> RemoveMemberOfActivity(Guid activityId, string userName)
        {
            var request = new RemoveMemberOfActivityCommand(activityId.ToString(), userName);
            var result = await _mediator.Send(request);

            return NewResult(result);
        }

        [HttpPut]
        [Route("UpdateActivity")]
        //[Authorize(CalendarClaims.UpdateActivity)]
        public async Task<IActionResult> UpdateActivity([FromBody] UpdateActivityRequest activityRequest)
        {
            var request = new UpdateActivityCommand(activityRequest);
            var result = await _mediator.Send(request);

            return NewResult(result);
        }

        [HttpPut]
        [Route("CompleteActivity")]
        //[Authorize(CalendarClaims.CompleteActivity)]
        public async Task<IActionResult> CompleteActivity(Guid activityId)
        {
            var request = new CompleteActivityCommand(activityId.ToString());
            var result = await _mediator.Send(request);

            return NewResult(result);
        }

        [HttpGet]
        [Route("GetUserActivity")]
        //[Authorize(CalendarClaims.GetAllUserActivity)]
        public async Task<IActionResult> GetAllUserActivity(DateTime? startDate, ActivityCategory? activityCategory
            , bool UserIsOwner, bool isCompleted, bool isHistory)
        {
            var request = new GetUserActivitiesQuery
                (startDate, UserIsOwner, isCompleted, isHistory, activityCategory);
            var result = await _mediator.Send(request);

            return NewResult(result);
        }

        [HttpGet]
        [Route("GetMemberOfActivity{activityId:guid}")]
        //[Authorize(CalendarClaims.GetMemberOfActivity)]
        public async Task<IActionResult> GetMemberOfActivity(Guid activityId)
        {
            var request = new GetMemberOfActivityQuery(activityId.ToString());
            var result = await _mediator.Send(request);

            return NewResult(result);
        }
    }
}
