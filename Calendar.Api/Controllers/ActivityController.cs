using Calendar.Api.Base;
using Core.Application.DTOs.ActivityDTOs;
using Core.Application.Features.Activities.Commands;
using Core.Application.Features.Activities.Queries;
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
        public async Task<IActionResult> CreateActivity([FromBody] CreateActivityRequest activityRequest)
        {
            var request = new CreateActivityCommand { createActivityRequest = activityRequest };
            var result = await _mediator.Send(request);

            return NewResult(result);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteActivity([FromRoute] Guid id)
        {
            var request = new DeleteActivityCommand { Id = id.ToString() };
            var result = await _mediator.Send(request);

            return NewResult(result);
        }
        
        [HttpDelete]
        [Route("ExitingActivity{id:guid}")]
        public async Task<IActionResult> ExitingActivity([FromRoute] Guid id)
        {
            var request = new ExitingActivityCommand { ActivityId = id.ToString() };
            var result = await _mediator.Send(request);

            return NewResult(result);
        }

        [HttpDelete]
        [Route("RemoveMemberOfActivity")]
        public async Task<IActionResult> RemoveMemberOfActivity(Guid activityId , string userName)
        {
            var request = new RemoveMemberOfActivityCommand { ActivityId = activityId.ToString() , UserName  = userName };
            var result = await _mediator.Send(request);

            return NewResult(result);
        }

        [HttpPut]
        [Route("UpdateActivity")]
        public async Task<IActionResult> UpdateActivity([FromBody] UpdateActivityRequest activityRequest)
        {
            var request = new UpdateActivityCommand { UpdateActivityRequest = activityRequest };
            var result = await _mediator.Send(request);

            return NewResult(result);
        }

        [HttpPut]
        [Route("CompleteActivity")]
        public async Task<IActionResult> CompleteActivity(Guid activityId)
        {
            var request = new CompleteActivityCommand { ActivityId = activityId.ToString() };
            var result = await _mediator.Send(request);

            return NewResult(result);
        }

        [HttpGet]
        [Route("GetActivities")]
        public async Task<IActionResult> GetActivities()
        {
            var request = new GetCurrentActivityQuery();
            var result = await _mediator.Send(request);

            return NewResult(result);
        }

        [HttpGet]
        [Route("GetMemberOfActivity{activityId:guid}")]
        public async Task<IActionResult> GetMemberOfActivity(Guid activityId)
        {
            var request = new GetMemberOfActivityQuery { ActivityId = activityId.ToString()};
            var result = await _mediator.Send(request);

            return NewResult(result);
        }

        [HttpGet]
        [Route("GetActivitiesThatTheUserIsMemberOf")]
        public async Task<IActionResult> GetActivitiesThatTheUserIsMemberOf()
        {
            var request = new GetActivitiesThatTheUserIsMemberOfQuery { };
            var result = await _mediator.Send(request);

            return NewResult(result);
        }

        [HttpGet]
        [Route("GetActivitiesHistory")]
        public async Task<IActionResult> GetActivitiesHistory()
        {
            var request = new GetHistoryOfActivitiesQuery();
            var result = await _mediator.Send(request);

            return NewResult(result);
        }
    }
}
