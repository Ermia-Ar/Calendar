using Calendar.Api.Base;
using Core.Application.DTOs.ProjectDTOs;
using Core.Application.DTOs.UserRequestDTOs;
using Core.Application.Features.Projects.Command;
using Core.Application.Features.Projects.Query;
using Core.Application.Features.UserRequests.Queries;
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
        public async Task<IActionResult> CreateProject([FromBody] CreateProjectRequest createProject)
        {
            var request = new CreateProjectCommand { CreateProject = createProject };
            var result = await _mediator.Send(request);

            return NewResult(result);
        }

        [HttpPost]
        [Route("RequestAddMemberToProject")]
        public async Task<IActionResult> RequestAddMemberToProject([FromBody] SendProjectRequest projectRequest)
        {
            var request = new RequestAddMemberToProjectCommand { ProjectRequest = projectRequest };
            var result = await _mediator.Send(request);

            return NewResult(result);
        }

        [HttpGet]
        [Route("GetProjectMembers{projectId:guid}")]
        public async Task<IActionResult> GetMemberOfProject(Guid projectId)
        {
            var request = new GetMemberOfProjectQuery { ProjectId = projectId.ToString() };
            var result = await _mediator.Send(request);

            return NewResult(result);
        }

        [HttpGet]
        [Route("GetProjectActivities{projectId:guid}")]
        public async Task<IActionResult> GetProjectActivities(Guid projectId)
        {
            var request = new GetProjectActivitiesQuery { ProjectId = projectId.ToString() };
            var result = await _mediator.Send(request);

            return NewResult(result);
        }

        [HttpGet]
        [Route("GetProjectsThatTheUserIsMemberOf")]
        public async Task<IActionResult> GetProjectsThatTheUserIsMemberOf()
        {
            var request = new GetProjectsThatTheUserIsMemberOfQuery { };
            var result = await _mediator.Send(request);

            return NewResult(result);
        }

        [HttpGet]
        [Route("GettingProjectsOwnedByTheUser")]
        public async Task<IActionResult> GettingProjectsOwnedByTheUser()
        {
            var request = new GettingProjectsOwnedByTheUserQuery { };
            var result = await _mediator.Send(request);

            return NewResult(result);
        }

        [HttpGet]
        [Route("GetProjectsRequestsReceived")]
        public async Task<IActionResult> GetRequestsReceived()
        {
            var request = new GetProjectRequestsReceivedQuery();
            var result = await _mediator.Send(request);

            return NewResult(result);
        }

        [HttpGet]
        [Route("GetProjectsRequestsResponse")]
        public async Task<IActionResult> GetRequestsResponse()
        {
            var request = new GetProjectRequestsResponseQuery();
            var result = await _mediator.Send(request);

            return NewResult(result);
        }

        [HttpGet]
        [Route("GetUnAnsweredProjectsRequest")]
        public async Task<IActionResult> GetUnAnsweredRequest()
        {
            var request = new GetUnAnsweredProjectRequestQuery();
            var result = await _mediator.Send(request);

            return NewResult(result);
        }

        [HttpGet]
        [Route("GetProjectsResponsesUserSent")]
        public async Task<IActionResult> GetResponsesUserSent()
        {
            var request = new GetProjcetResponsesUserSentQuery();
            var result = await _mediator.Send(request);

            return NewResult(result);
        }

        [HttpDelete]
        [Route("ExitingProject{projectId:guid}")]
        public async Task<IActionResult> ExitingProject(Guid projectId)
        {
            var request = new ExitingProjectCommand { ProjectId = projectId.ToString() };
            var result = await _mediator.Send(request);

            return NewResult(result);
        }

        [HttpDelete]
        [Route("RemoveMemberOfProject")]
        public async Task<IActionResult> RemoveMemberOfProject(Guid projectId , string userName)
        {
            var request = new RemoveMemberOfProjectCommand { ProjectId= projectId.ToString() , UserName =  userName};
            var result = await _mediator.Send(request);

            return NewResult(result);
        }

        [HttpDelete]
        [Route("DeleteProject{projectId:guid}")]
        public async Task<IActionResult> DeleteProject(Guid projectId)
        {
            var request = new DeleteProjectCommand { ProjcetId = projectId.ToString() };
            var result = await _mediator.Send(request);

            return NewResult(result);
        }
    }
}
