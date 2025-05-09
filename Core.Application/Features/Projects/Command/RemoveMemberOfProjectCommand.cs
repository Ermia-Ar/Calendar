using Core.Domain.Shared;
using MediatR;

namespace Core.Application.Features.Projects.Command
{
    public class RemoveMemberOfProjectCommand : IRequest<Response<string>>
    {
        public string ProjectId { get; set; }

        public string UserName { get; set; }
    }
}
