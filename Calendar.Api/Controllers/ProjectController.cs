using Core.Application.ApplicationServices.Activities.Queries.GetById;
using Core.Application.DTOs.ProjectDTOs;
using Core.Application.DTOs.UserDTOs;
using Core.Application.Features.Projects.Command;
using Core.Application.Features.Projects.Query;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.ResponseResult;

namespace Calendar.Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private IMediator _mediator;

        public ProjectController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("CreateProject")]
        //[Authorize(CalendarClaims.CreateProject)]
        public async Task<SuccessResponse> CreateProject([FromBody] CreateProjectRequest createProject)
        {
            var request = new CreateProjectCommand(createProject);
            var result = await _mediator.Send(request);

            return Result.Ok();
        }

        [HttpGet]
        [Route("GetProjectMembers{projectId:guid}")]
        //[Authorize(CalendarClaims.GetMemberOfProject)]
        public async Task<SuccessResponse<List<UserResponse>>> GetMemberOfProject([FromRoute] Guid projectId)
        {
            var request = new GetMemberOfProjectQuery(projectId.ToString());
            var result = await _mediator.Send(request);
            return Result.Ok(result);
        }

        [HttpGet]
        [Route("GetActivitiesOfProject{projectId:guid}")]
        //[Authorize(CalendarClaims.GetActivitiesOfProject)]
        public async Task<SuccessResponse<List<GetByIdActivityQueryResponse>>> GetActivitiesOfProject([FromRoute] Guid projectId)
        {
            var request = new GetActivitiesOfProjectQuery(projectId.ToString());
            var result = await _mediator.Send(request);

            return Result.Ok(result);
        }

        [HttpGet]
        [Route("GetAllUserProjects")]
        //[Authorize(CalendarClaims.GetAllUserProjects)]
        public async Task<SuccessResponse<List<ProjectResponse>>> GetAllUserProjects(DateTime? startDate, bool userIsOwner, bool isHistory)
        {
            var request = new GetUserProjectsQuery(startDate, userIsOwner, isHistory);
            var result = await _mediator.Send(request);

            return Result.Ok(result);
        }

        [HttpGet]
        [Route("GetProjectById")]
        public async Task<SuccessResponse<ProjectResponse>> GetProjectById(Guid id)
        {
            var request = new GetProjectByIdQuery(id.ToString());
            var result = await _mediator.Send(request);

            return Result.Ok(result);
        }

        [HttpDelete]
        [Route("ExitingProject{projectId:guid}")]
        //[Authorize(CalendarClaims.ExitingProject)]
        public async Task<SuccessResponse> ExitingProject(Guid projectId)
        {
            var request = new ExitingProjectCommand(projectId.ToString());
            var result = await _mediator.Send(request);
            return Result.Ok();
        }

        [HttpDelete]
        [Route("RemoveMemberOfProject")]
        //[Authorize(CalendarClaims.RemoveMemberOfProject)]
        public async Task<SuccessResponse> RemoveMemberOfProject(Guid projectId, string userName)
        {
            var request = new RemoveMemberOfProjectCommand(projectId.ToString(), userName);
            var result = await _mediator.Send(request);
            return Result.Ok();
        }

        [HttpDelete]
        [Route("DeleteProject{projectId:guid}")]
        //[Authorize(CalendarClaims.DeleteProject)]
        public async Task<SuccessResponse> DeleteProject(Guid projectId)
        {
            var request = new DeleteProjectCommand(projectId.ToString());
            var result = await _mediator.Send(request);
            return Result.Ok();
        }
    }
}
