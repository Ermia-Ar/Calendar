using Calendar.Api.Base;
using Core.Application.DTOs.UserRequestDTOs;
using Core.Application.Features.UserRequests.Commnads;
using Core.Application.Features.UserRequests.Queries;
using Core.Domain.Enum;
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
        //[Authorize(CalendarClaims.SendRequest)]
        public async Task<IActionResult> SendActivityRequest([FromBody] SendActivityRequest sendRequest)
        {
            var request = new CreateRequestCommand { Request = sendRequest };
            var result = await _mediator.Send(request);
            return NewResult(result);
        }

        [HttpPost]
        [Route("AnswerRequest")]
        //[Authorize(CalendarClaims.AnswerRequest)]
        public async Task<IActionResult> AnswerRequest([FromForm] string requestId, [FromForm] bool isAccepted)
        {
            var request = new AnswerRequestCommand { RequestId = requestId, IsAccepted = isAccepted };
            var result = await _mediator.Send(request);
            return NewResult(result);
        }

        [HttpDelete]
        [Route("RemoveRequest{id:guid}")]
        //[Authorize(CalendarClaims.RemoveRequest)]
        public async Task<IActionResult> RemoveRequest([FromRoute] Guid id)
        {
            var request = new DeleteRequestCommand { Id = id.ToString() };
            var result = await _mediator.Send(request);
            return NewResult(result);
        }

        [HttpGet]
        [Route("GetRequestsReceived")]
        //[Authorize(CalendarClaims.GetRequestsReceived)]
        public async Task<IActionResult> GetRequestsReceived(RequestFor? requestFor)
        {
            var request = new GetRequestsReceivedQuery { RequestFor = requestFor };
            var result = await _mediator.Send(request);
            return NewResult(result);
        }

        [HttpGet]
        [Route("GetRequestsResponse")]
        //[Authorize(CalendarClaims.GetRequestsResponse)]
        public async Task<IActionResult> GetRequestsResponse(RequestFor? requestFor)
        {
            var request = new GetRequestsResponseQuery { RequestFor = requestFor };
            var result = await _mediator.Send(request);
            return NewResult(result);
        }

        [HttpGet]
        [Route("GetUnAnsweredRequest")]
        //[Authorize(CalendarClaims.GetUnAnsweredRequest)]
        public async Task<IActionResult> GetUnAnsweredRequest(RequestFor? requestFor)
        {
            var request = new GetUnAnsweredRequestQuery { RequestFor = requestFor };
            var result = await _mediator.Send(request);
            return NewResult(result);
        }

        [HttpGet]
        [Route("GetResponsesUserSent")]
        //[Authorize(CalendarClaims.GetResponsesUserSent)]
        public async Task<IActionResult> GetResponsesUserSent(RequestFor? requestFor)
        {
            var request = new GetResponsesUserSentQuery { RequestFor = requestFor };
            var result = await _mediator.Send(request);
            return NewResult(result);
        }
    }
}
