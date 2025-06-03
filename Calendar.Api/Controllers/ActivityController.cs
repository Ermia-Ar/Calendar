using Core.Application.ApplicationServices.Activities.Queries.GetById;
using Core.Application.DTOs.ActivityDTOs;
using Core.Application.DTOs.UserDTOs;
using Core.Application.Features.Activities.Commands;
using Core.Application.Features.Activities.Queries;
using Core.Domain.Enum;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.ResponseResult;

namespace Calendar.Api.Controllers;

[Route("api/[controller]/")]
[ApiController]
[Authorize]
public class ActivityController : ControllerBase
{
    private IMediator _mediator;

    public ActivityController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Route("CreateActivity")]
    //[Authorize(CalendarClaims.CreateActivity)]
    public async Task<SuccessResponse> CreateActivity([FromBody] CreateActivityRequest activityRequest)
    {
        var request = new CreateActivityCommand (activityRequest);
        var result = await _mediator.Send(request);

        return Result.Ok();
    }
    
    [HttpPost]
    [Route("CreateActivityForProject")]
    //[Authorize(CalendarClaims.CreateActivityForProject)]
    public async Task<SuccessResponse> CreateActivityForProject([FromBody] CreateActivityForProjectRequest activityRequest)
    {
        var request = new AddActivityForProjectCommand {CreateActivity = activityRequest };
        var result = await _mediator.Send(request);

        return Result.Ok();
    }

    [HttpPost]
    [Route("CreateSubActivity")]
    //[Authorize(CalendarClaims.CreateSubActivity)]
    public async Task<SuccessResponse> CreateSubActivity([FromBody] CreateSubActivityRequest activityRequest)
    {
        var request = new CreateSubActivityCommand { CreateActivity = activityRequest};
        var result = await _mediator.Send(request);

        return Result.Ok();
    }

    [HttpDelete]
    [Route("DeleteActivity{id:guid}")]
    //[Authorize(CalendarClaims.DeleteActivity)]
    public async Task<SuccessResponse> DeleteActivity([FromRoute] Guid id)
    {
        var request = new DeleteActivityCommand (id.ToString());
        var result = await _mediator.Send(request);

        return Result.Ok();
    }

    [HttpDelete]
    [Route("ExitingActivity{id:guid}")]
    //[Authorize(CalendarClaims.ExitingActivity)]
    public async Task<SuccessResponse> ExitingActivity([FromRoute] Guid id)
    {
        var request = new ExitingActivityCommand (id.ToString());
        var result = await _mediator.Send(request);

        return Result.Ok();
    }

    [HttpDelete]
    [Route("RemoveMemberOfActivity")]
    //[Authorize(CalendarClaims.RemoveMemberOfActivity)]
    public async Task<SuccessResponse> RemoveMemberOfActivity(Guid activityId, string userName)
    {
        var request = new RemoveMemberOfActivityCommand(activityId.ToString(), userName);
        var result = await _mediator.Send(request);

        return Result.Ok();
    }

    [HttpPut]
    [Route("UpdateActivity")]
    //[Authorize(CalendarClaims.UpdateActivity)]
    public async Task<SuccessResponse> UpdateActivity([FromBody] UpdateActivityRequest activityRequest)
    {
        var request = new UpdateActivityCommand(activityRequest);
        var result = await _mediator.Send(request);

        return Result.Ok();
    }

    [HttpPut]
    [Route("CompleteActivity")]
    //[Authorize(CalendarClaims.CompleteActivity)]
    public async Task<SuccessResponse> CompleteActivity(Guid activityId)
    {
        var request = new CompleteActivityCommand(activityId.ToString());
        var result = await _mediator.Send(request);

        return Result.Ok();
    }

    [HttpGet]
    [Route("GetUserActivity")]
    //[Authorize(CalendarClaims.GetAllUserActivity)]
    public async Task<SuccessResponse<List<GetByIdActivityQueryResponse>>> GetAllUserActivity(DateTime? startDate, ActivityCategory? activityCategory
        , bool UserIsOwner, bool isCompleted, bool isHistory)
    {
        var request = new GetUserActivitiesQuery
            (startDate, UserIsOwner, isCompleted, isHistory, activityCategory);
        var result = await _mediator.Send(request);

        return Result.Ok(result);
    }

    [HttpGet]
    [Route("GetMemberOfActivity{activityId:guid}")]
    //[Authorize(CalendarClaims.GetMemberOfActivity)]
    public async Task<SuccessResponse<List<UserResponse>>> GetMemberOfActivity(Guid activityId)
    {
        var request = new GetMemberOfActivityQuery(activityId.ToString());
        var result = await _mediator.Send(request);

        return Result.Ok(result);
    }

    [HttpGet]
    [Route("GetActivityById{id:guid}")]
    public async Task<SuccessResponse<GetByIdActivityQueryResponse>> GetActivityById(Guid id)
    {
        var request = new GetActivityByIdQuery(id.ToString());
        var result = await _mediator.Send(request);

        return Result.Ok(result);
    }
}
