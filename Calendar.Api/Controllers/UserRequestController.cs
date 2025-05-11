using Calendar.Api.Base;
using Core.Application.DTOs.UserRequestDTOs;
using Core.Application.Features.UserRequests.Commnads;
using Core.Application.Features.UserRequests.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Calendar.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "User")]
    public class UserRequestController : AppControllerBase
    {
        private IMediator _mediator;

        public UserRequestController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("SendActivityRequest")]
        public async Task<IActionResult> SendRequest([FromBody] SendActivityRequest sendRequest)
        {
            var request = new CreateRequestCommand { Request = sendRequest };
            var result = await _mediator.Send(request);

            return NewResult(result);
        }

        [HttpPost]
        [Route("AnswerActivityRequest")]
        public async Task<IActionResult> AnswerRequest([FromForm] string requestId ,[FromForm] bool isAccepted)
        {
            var request = new AnswerRequestCommand {RequestId = requestId , IsAccepted = isAccepted};
            var result = await _mediator.Send(request);

            return NewResult(result);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> RemoveRequest([FromRoute] Guid id)
        {
            var request = new DeleteRequestCommand { Id = id.ToString() };
            var result = await _mediator.Send(request);

            return NewResult(result);
        }

        [HttpGet]
        [Route("GetActivityRequestsReceived")]
        public async Task<IActionResult> GetRequestsReceived()
        {
            var request = new GetRequestsReceivedQuery();
            var result = await _mediator.Send(request);

            return NewResult(result); 
        }

        [HttpGet]
        [Route("GetActivityRequestsResponse")]
        public async Task<IActionResult> GetRequestsResponse()
        {
            var request = new GetRequestsResponseQuery();
            var result = await _mediator.Send(request);

            return NewResult(result);
        }

        [HttpGet]
        [Route("GetUnAnsweredActivityRequest")]
        public async Task<IActionResult> GetUnAnsweredRequest()
        {
            var request = new GetUnAnsweredRequestQuery();
            var result = await _mediator.Send(request);

            return NewResult(result);
        }
        
        [HttpGet]
        [Route("GetActivityResponsesUserSent")]
        public async Task<IActionResult> GetResponsesUserSent()
        {
            var request = new GetResponsesUserSentQuery();
            var result = await _mediator.Send(request);

            return NewResult(result);
        }

    }
}
