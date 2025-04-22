using Calendar.Api.Base;
using Core.Application.DTOs.ActivityDTOs;
using Core.Application.Features.Activity.Commands;
using Core.Application.Features.Activity.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Calendar.Api.Controllers
{
    [Route("api/[controller]")]
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
        public async Task<IActionResult> CreateActivity([FromBody]CreateActivityRequest activityRequest)
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

        [HttpPut]
        [Route("UpdateActivity")]
        public async Task<IActionResult> CreateActivity([FromBody] UpdateActivityRequest activityRequest)
        {
            var request = new UpdateActivityCommand { UpdateActivityRequest = activityRequest };
            var result = await _mediator.Send(request);

            return NewResult(result);
        }

        [HttpGet]
        [Route("GetActivities")]
        public async Task<IActionResult> CreateActivity()
        {
            var request = new GetCurrentActivityQuery();
            var result = await _mediator.Send(request);

            return NewResult(result);
        }

    }
}
