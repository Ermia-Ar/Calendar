using Core.Application.DTOs.ProjectDTOs;
using Core.Domain.Shared;
using MediatR;

namespace Core.Application.Features.Projects.Query
{
    public class GetAllUserProjectQuery : IRequest<Response<List<ProjectResponse>>>
    {
        public DateTime? StartDate { get; set; }
        public bool UserIsOwner { get; set; }
        public bool IsHistory {  get; set; }
    }
}