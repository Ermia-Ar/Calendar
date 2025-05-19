using Calendar.Api.Base;
using Core.Application.DTOs.ProjectDTOs;
using Core.Application.DTOs.UserRequestDTOs;
using Core.Application.Features.Projects.Command;
using Core.Application.Features.Projects.Query;
using Core.Domain.Helper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Calendar.Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ProjectController : AppControllerBase
    {
        private IMediator _mediator;

        public ProjectController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("CreateProject")]
        [Authorize(CalendarClaims.CreateProject)]
        public async Task<IActionResult> CreateProject([FromBody] CreateProjectRequest createProject)
        {
            var request = new CreateProjectCommand { CreateProject = createProject };
            var result = await _mediator.Send(request);
            return NewResult(result);
        }

        [HttpPost]
        [Route("RequestAddMemberToProject")]
        [Authorize(CalendarClaims.RequestAddMemberToProject)]
        public async Task<IActionResult> RequestAddMemberToProject([FromBody] SendProjectRequest projectRequest)
        {
            var request = new RequestAddMemberToProjectCommand { ProjectRequest = projectRequest };
            var result = await _mediator.Send(request);
            return NewResult(result);
        }

        [HttpGet]
        [Route("GetProjectMembers{projectId:guid}")]
        [Authorize(CalendarClaims.GetMemberOfProject)]
        public async Task<IActionResult> GetMemberOfProject([FromRoute] Guid projectId)
        {
            var request = new GetMemberOfProjectQuery { ProjectId = projectId.ToString() };
            var result = await _mediator.Send(request);
            return NewResult(result);
        }

        [HttpGet]
        [Route("GetActivitiesOfProject{projectId:guid}")]
        [Authorize(CalendarClaims.GetActivitiesOfProject)]
        public async Task<IActionResult> GetActivitiesOfProject([FromRoute] Guid projectId)
        {
            var request = new GetActivitiesOfProjectQuery { ProjectId = projectId.ToString() };
            var result = await _mediator.Send(request);
            return NewResult(result);
        }

        [HttpGet]
        [Route("GetAllUserProjects")]
        [Authorize(CalendarClaims.GetAllUserProjects)]
        public async Task<IActionResult> GetAllUserProjects(DateTime? startDate, bool userIsOwner, bool isHistory)
        {
            var request = new GetAllUserProjectQuery
            {
                IsHistory = isHistory,
                StartDate = startDate,
                UserIsOwner = userIsOwner
            };
            var result = await _mediator.Send(request);
            return NewResult(result);
        }

        [HttpDelete]
        [Route("ExitingProject{projectId:guid}")]
        [Authorize(CalendarClaims.ExitingProject)]
        public async Task<IActionResult> ExitingProject(Guid projectId)
        {
            var request = new ExitingProjectCommand { ProjectId = projectId.ToString() };
            var result = await _mediator.Send(request);
            return NewResult(result);
        }

        [HttpDelete]
        [Route("RemoveMemberOfProject")]
        [Authorize(CalendarClaims.RemoveMemberOfProject)]
        public async Task<IActionResult> RemoveMemberOfProject(Guid projectId, string userName)
        {
            var request = new RemoveMemberOfProjectCommand { ProjectId = projectId.ToString(), UserName = userName };
            var result = await _mediator.Send(request);
            return NewResult(result);
        }

        [HttpDelete]
        [Route("DeleteProject{projectId:guid}")]
        [Authorize(CalendarClaims.DeleteProject)]
        public async Task<IActionResult> DeleteProject(Guid projectId)
        {
            var request = new DeleteProjectCommand { ProjcetId = projectId.ToString() };
            var result = await _mediator.Send(request);
            return NewResult(result);
        }
    }
}
