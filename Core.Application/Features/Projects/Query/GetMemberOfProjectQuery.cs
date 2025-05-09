using Core.Domain.Shared;
using MediatR;

namespace Core.Application.Features.Projects.Query
{
    public class GetMemberOfProjectQuery : IRequest<Response<List<string>>>
    {
        public string ProjectId { get; set; }   
    }
}
