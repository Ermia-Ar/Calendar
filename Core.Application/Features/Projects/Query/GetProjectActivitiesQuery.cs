using Core.Application.DTOs.ActivityDTOs;
using Core.Domain.Shared;
using MediatR;

namespace Core.Application.Features.Projects.Query
{
    public class GetProjectActivitiesQuery : IRequest<Response<List<ActivityResponse>>>
    {
        public string ProjectId { get; set; }
    }
}
