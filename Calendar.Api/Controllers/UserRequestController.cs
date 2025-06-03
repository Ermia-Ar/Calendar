using Core.Application.DTOs.UserRequestDTOs;
using Core.Application.Features.Projects.Command;
using Core.Application.Features.UserRequests.Commnads;
using Core.Application.Features.UserRequests.Queries;
using Core.Domain.Enum;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.ResponseResult;

namespace Calendar.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "User")]
    public class UserRequestController : ControllerBase
    {
        private IMediator _mediator;

        public UserRequestController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost]
        [Route("SendProjectRequest")]
        //[Authorize(CalendarClaims.SendProjectRequest)]
        public async Task<SuccessResponse> RequestAddMemberToProject([FromBody] SendProjectRequest projectRequest)
        {
            var request = new RequestAddMemberToProjectCommand(projectRequest);
            var result = await _mediator.Send(request);
            return Result.Ok();
        }

        [HttpPost]
        [Route("SendActivityRequest")]
        //[Authorize(CalendarClaims.SendActivityRequest)]
        public async Task<SuccessResponse> SendActivityRequest([FromBody] SendActivityRequest activityRequest)
        {
            var request = new CreateActivityRequestCommand(activityRequest);
            var result = await _mediator.Send(request);
            return Result.Ok();
        }

        [HttpPost]
        [Route("AnswerRequest")]
        //[Authorize(CalendarClaims.AnswerRequest)]
        public async Task<SuccessResponse> AnswerRequest(string requestId, bool isAccepted)
        {
            var request = new AnswerRequestCommand(requestId,isAccepted);
            var result = await _mediator.Send(request);
            return Result.Ok();
        }

        [HttpDelete]
        [Route("RemoveRequest{id:guid}")]
        //[Authorize(CalendarClaims.RemoveRequest)]
        public async Task<SuccessResponse> RemoveRequest([FromRoute] Guid id)
        {
            var request = new DeleteRequestCommand(id.ToString());
            var result = await _mediator.Send(request);
            return Result.Ok();
        }

        [HttpGet]
        [Route("GetRequestsReceived")]
        //[Authorize(CalendarClaims.GetRequestsReceived)]
        public async Task<SuccessResponse<List<RequestResponse>>> GetRequestsReceived(RequestFor? requestFor)
        {
            var request = new GetRequestsReceivedQuery (requestFor);
            var result = await _mediator.Send(request);
            return Result.Ok(result);
        }

        [HttpGet]
        [Route("GetRequestsResponse")]
        //[Authorize(CalendarClaims.GetRequestsResponse)]
        public async Task<SuccessResponse<List<RequestResponse>>> GetRequestsResponse(RequestFor? requestFor)
        {
            var request = new GetRequestsResponseQuery (requestFor);
            var result = await _mediator.Send(request);
            return Result.Ok(result);
        }

        [HttpGet]
        [Route("GetUnAnsweredRequest")]
        //[Authorize(CalendarClaims.GetUnAnsweredRequest)]
        public async Task<SuccessResponse<List<RequestResponse>>> GetUnAnsweredRequest(RequestFor? requestFor)
        {
            var request = new GetUnAnsweredRequestQuery (requestFor);
            var result = await _mediator.Send(request);
            return Result.Ok(result);
        }

        [HttpGet]
        [Route("GetResponsesUserSent")]
        //[Authorize(CalendarClaims.GetResponsesUserSent)]
        public async Task<SuccessResponse<List<RequestResponse>>> GetResponsesUserSent(RequestFor? requestFor)
        {
            var request = new GetResponsesUserSentQuery (requestFor);
            var result = await _mediator.Send(request);
            return Result.Ok(result);
        }
    }
}
